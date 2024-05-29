using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using UnityEngine;
using UnityEngine.AI;

namespace Aplib.Integrations.Unity.Actions
{
    /// <summary>
    /// A special action, which will use a NavMesh to determine where a given object needs to move towards, and
    /// executes a given action with that information.
    /// </summary>
    /// <typeparam name="TBeliefSet">The type of belief set this has to work with.</typeparam>
    /// <remarks>This does not move anything, it just calculates required information.</remarks>
    public class UnityPathfinderAction<TBeliefSet> : Core.Intent.Actions.Action<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Creates a special action, which will use a NavMesh to determine where a given object needs to move towards, and
        /// executes a given action with that information.
        /// </summary>
        /// <param name="metadata">Helpful metadata to specify what this action is used for</param>
        /// <param name="objectQuery">The function which determines the object to move around.</param>
        /// <param name="location">The function which determines the target destination of the path-finding.</param>
        /// <param name="effect">
        /// Given the belief set and the next position determined by the NavMesh agent, this arbitraty effect will
        /// be aplied with said information.
        /// </param>
        /// <param name="heightOffset">An optional offset to correct for an objects height during pathfinding.</param>
        public UnityPathfinderAction(
            Metadata metadata,
            System.Func<TBeliefSet, Rigidbody> objectQuery,
            System.Func<TBeliefSet, Vector3> location,
            System.Action<TBeliefSet, Vector3> effect,
            float heightOffset = 0f)
            : base(metadata, PathfindingAction(objectQuery, location, effect, heightOffset))
        { }

        /// <inheritdoc cref="UnityPathfinderAction{TBeliefSet}(Metadata,System.Func{TBeliefSet,Rigidbody},System.Func{TBeliefSet,UnityEngine.Vector3},System.Action{TBeliefSet,Vector3},float)"/>
        public UnityPathfinderAction(
            System.Func<TBeliefSet, Rigidbody> objectQuery,
            System.Func<TBeliefSet, Vector3> location,
            System.Action<TBeliefSet, Vector3> effect,
            float heightOffset = 0f)
            : this(new Metadata(), objectQuery, location, effect, heightOffset)
        { }

        /// <inheritdoc cref="UnityPathfinderAction{TBeliefSet}(Metadata,System.Func{TBeliefSet,Rigidbody},System.Func{TBeliefSet,UnityEngine.Vector3},System.Action{TBeliefSet,Vector3},float)"/>
        public UnityPathfinderAction(
            Metadata metadata,
            System.Func<TBeliefSet, Rigidbody> objectQuery,
            Vector3 location,
            System.Action<TBeliefSet, Vector3> effect,
            float heightOffset = 0f)
            : this(metadata, objectQuery, location: ConstantLocation(location), effect, heightOffset)
        { }

        /// <inheritdoc cref="UnityPathfinderAction{TBeliefSet}(Metadata,System.Func{TBeliefSet,Rigidbody},System.Func{TBeliefSet,UnityEngine.Vector3},System.Action{TBeliefSet,Vector3},float)"/>
        public UnityPathfinderAction(
            System.Func<TBeliefSet, Rigidbody> objectQuery,
            Vector3 location,
            System.Action<TBeliefSet, Vector3> effect,
            float heightOffset = 0f)
            : this(new Metadata(), objectQuery, location: ConstantLocation(location), effect, heightOffset)
        { }


        /// <summary>
        /// Simply wraps a constant location in a function.
        /// </summary>
        /// <param name="location">The location to always return.</param>
        /// <returns>The constant location.</returns>
        protected static System.Func<TBeliefSet, Vector3> ConstantLocation(Vector3 location) => _ => location;

        private static System.Action<TBeliefSet> PathfindingAction(System.Func<TBeliefSet, Rigidbody> objectQuery,
            System.Func<TBeliefSet, Vector3> locationQuery,
            System.Action<TBeliefSet, Vector3> effect,
            float speed = 7f,
            float heightOffset = 0f) => beliefSet =>
        {
            Rigidbody rigidbody = objectQuery(beliefSet);
            Vector3 target = locationQuery(beliefSet);

            NavMeshPath path = new();
            NavMesh.CalculatePath(rigidbody.position,
                target,
                NavMesh.AllAreas,
                path
            );

            if (path.corners.Length <= 1) return;

            // Calculate and indicate in what direction we should move.
            Vector3 targetPosition = path.corners[1] + Vector3.up * heightOffset;
            Vector3 newPosition = Vector3.MoveTowards(
                rigidbody.position, targetPosition, maxDistanceDelta: Time.deltaTime * speed);
            Debug.DrawLine(targetPosition, rigidbody.position, Color.blue);

            // Call the effect with the new position and direction
            effect(beliefSet, newPosition);
        };
    }
}
