using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine
{
    private IState _currentState;

    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();

    // Transitions relevant to the current state
    private List<Transition> _currentTransitions = new List<Transition>();

    // Transitions relevant to any state
    private List<Transition> _anyTransitions = new List<Transition>();

    // 
    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    public void Tick()
    {
        // Check if there is a valid transition
        var transition = GetTransition();
        if(transition != null)
        {
            SetState(transition.To);
        }

        _currentState?.Tick();
    }

    public void SetState(IState state)
    {
        if (state == _currentState) { return; } // To prevent exiting and entering the same state

        _currentState?.Exit();
        _currentState = state;

        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        if (_currentTransitions == null)
        {
            // Prevent future null reference exceptions
            // Use EmptyTransitions to prevent unnecessary allocations
            _currentTransitions = EmptyTransitions;
        }

        _currentState.Enter();
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if(_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            // From state is not already in dictionary, so add it
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        _anyTransitions.Add(new Transition(state, predicate));
    }

    private Transition GetTransition()
    {
        // Based on the order that transitions are added
        // Maybe use a priority system if needed, but shouldn't be necessary

        foreach(var transition in _anyTransitions)
        {
            if(transition.Condition())
            {
                return transition;
            }
        }

        foreach(var transition in _currentTransitions)
        {
            if(transition.Condition())
            {
                return transition;
            }
        }
        return null;
    }
    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}
