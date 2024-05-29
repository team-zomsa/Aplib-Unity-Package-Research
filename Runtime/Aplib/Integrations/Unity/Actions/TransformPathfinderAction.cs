using Aplib.Core;
using Aplib.Core.Belief.BeliefSets;
using UnityEngine;

namespace Aplib.Integrations.Unity.Actions
{
    /// <summary>
    /// A special action which uses a NavMesh to move and rotate a transform towards a target location.
    /// </summary>
    /// <typeparam name="TBeliefSet">The type of belief set this has to work with.</typeparam>
    public class TransformPathfinderAction<TBeliefSet> : UnityPathfinderAction<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Creates a special action which uses a NavMesh to move and rotate a transform towards a target location.
        /// </summary>
        /// <param name="metadata">Helpful metadata to specify what this action is used for</param>
        /// <param name="objectQuery">The function which determines the object to move around.</param>
        /// <param name="location">The function which determines the target destination of the path-finding.</param>
        /// <param name="heightOffset">An optional offset to correct for an objects height during pathfinding.</param>
        public TransformPathfinderAction(
            Metadata metadata,
            System.Func<TBeliefSet, Rigidbody> objectQuery,
            System.Func<TBeliefSet, Vector3> location,
            float heightOffset = 0f)
            : base(metadata, objectQuery, location, effect: PathfindingAction(objectQuery), heightOffset)
        { }

        /// <inheritdoc cref="TransformPathfinderAction{TBeliefSet}(Metadata,System.Func{TBeliefSet,Rigidbody},System.Func{TBeliefSet,Vector3},float)"/>
        public TransformPathfinderAction(
            System.Func<TBeliefSet, Rigidbody> objectQuery,
            System.Func<TBeliefSet, Vector3> location,
            float heightOffset = 0f)
            : this(new Metadata(), objectQuery, location, heightOffset)
        { }

        /// <inheritdoc cref="TransformPathfinderAction{TBeliefSet}(Metadata,System.Func{TBeliefSet,Rigidbody},System.Func{TBeliefSet,Vector3},float)"/>
        public TransformPathfinderAction(
            Metadata metadata,
            System.Func<TBeliefSet, Rigidbody> objectQuery,
            Vector3 location,
            float heightOffset = 0f)
            : this(metadata, objectQuery, location: ConstantLocation(location), heightOffset)
        { }

        /// <inheritdoc cref="TransformPathfinderAction{TBeliefSet}(Metadata,System.Func{TBeliefSet,Rigidbody},System.Func{TBeliefSet,Vector3},float)"/>
        public TransformPathfinderAction(
            System.Func<TBeliefSet, Rigidbody> objectQuery,
            Vector3 location,
            float heightOffset = 0f)
            : this(new Metadata(), objectQuery, location: ConstantLocation(location), heightOffset)
        { }

        private static System.Action<TBeliefSet, Vector3> PathfindingAction(System.Func<TBeliefSet, Rigidbody> objectQuery)
            => (beliefSet, destination) =>
            {
                Rigidbody rigidbody = objectQuery(beliefSet);

                // Calculate the new direction
                Vector3 direction = destination - rigidbody.position;
                direction = new Vector3(direction.x, 0, direction.z);
                direction.Normalize();

                rigidbody.position = destination;

                // If the direction is zero, don't rotate the object
                if (direction == Vector3.zero) return;

                rigidbody.rotation = Quaternion.LookRotation(direction);
            };
    }
}
