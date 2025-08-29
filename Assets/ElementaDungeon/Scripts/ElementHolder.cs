using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using WhaleTee.Reactive.Runtime.Input;
using WhaleTee.Runtime.Extensions;
using WhaleTee.Runtime.ServiceLocator;

public class ElementHolder : MonoBehaviour {
  [SerializeField] private int capacity;
  [SerializeField] private float clickTimeToMixElements;
  [ShowInInspector] public readonly Stack<ElementData> elements = new Stack<ElementData>();

  private UserInput userInput;
  private float clickDuration;

  private void Awake() {
    ServiceLocator.Global.TryGet(out userInput);
  }

  private void Update() {
    if (userInput.Click.Value) {
      clickDuration += Time.deltaTime;
    } else {
      if (clickDuration > 0) {
        if (clickDuration < clickTimeToMixElements) GatherElement();
        else MixElements();
        clickDuration = 0;
      }
    }
  }

  private void MixElements() {
    if (elements.Count > 1) {
      var one = elements.Pop();
      var two = elements.Pop();
      var elementInteraction = ElementSystem.Instance.GetInteraction(one, two);
      
      if (elementInteraction) elements.Push(elementInteraction.resultElement);
      else {
        elements.Push(one);
        elements.Push(two);
      }
    }
  }

  private void GatherElement() {
    var cell = SceneObjectFinder.FindObjectByPosition2DAndType<ElementalCell>(transform.position);

    if (cell && cell.initElement.OrNull() != null) {
      if (elements.Count < capacity) {
        elements.Push(cell.initElement);
      }
    }
  }

  public ElementData PopElement() => elements.Count > 0 ? elements.Pop() : null;
}