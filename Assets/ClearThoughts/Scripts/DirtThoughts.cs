using System.Collections.Generic;
using R3;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DirtThoughts : MonoBehaviour {
  public ReactiveProperty<int> thoughtsCount = new ReactiveProperty<int>(5);
  public PullThought pullThoughtPrefab;
  private Animator animator;
  private Queue<PullThought> pullThoughts = new Queue<PullThought>();

  private void Awake() {
    animator = GetComponent<Animator>();

    thoughtsCount
    .Subscribe(count => {
                 switch (count) {
                   case 5: animator.Play(TangleAnimations.tangle5); break;
                   case 4: animator.Play(TangleAnimations.tangle4); break;
                   case 3: animator.Play(TangleAnimations.tangle3); break;
                   case 2: animator.Play(TangleAnimations.tangle2); break;
                   case 1: animator.Play(TangleAnimations.tangle1); break;
                 }

                 if (count > 0) pullThoughts.Dequeue().gameObject.SetActive(true);
               }
    );

    for (var i = 0; i < thoughtsCount.Value; i++) {
      var pullThought = Instantiate(pullThoughtPrefab, transform);
      pullThought.gameObject.SetActive(false);
      pullThoughts.Enqueue(pullThought);
    }

    pullThoughts.Dequeue().gameObject.SetActive(true);
  }
}