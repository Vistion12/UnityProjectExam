using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField] Text _text;
    private int _counter = 0;
    public delegate void OnMaxReached();
    public event OnMaxReached MaxReached;

    public int Value => _counter;

    public void CounterAdd()
    {
        _counter++;
        Debug.Log($"[Counter] Add! Счётчик теперь: {_counter}");
        _text.text = _counter.ToString();
        if (_counter > 3)
        {
            Debug.Log("[Counter] Достигнуто >3, вызываем MaxReached");
            //_counter = 0;
            _text.text = _counter.ToString();

            if (MaxReached != null)
            {
                MaxReached.Invoke();
            }
        }
    }
}