// whileTrueIterate v1.0
// Modified by __Darkhitori__
// __ECO__ __PLAYMAKER__ __ACTION__

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("While the Bool is True. \n" +
	"This lets you quickly loop an Event and send an Finished Event when done or send a Failed Event if action fails.\n")]
	public class WhileTrueIterate : ArrayListActions
	{
		[ActionSection("While")]
		[Tooltip("When true starts iteration. When variable is false resets Index back to 0")]
		[Title("Bool")]
		[UIHint(UIHint.Variable)]
		public FsmBool whileBool;
		
		[ActionSection("Iterate")]
		[Tooltip("From where to start iteration, leave to 0 to start from the beginning")]
		public FsmInt startIndex;
		
		[Tooltip("When to end iteration")]
		public FsmInt endIndex;
		
		[Tooltip("Event to send to get the next child.")]
		public FsmEvent loopEvent;

		[Tooltip("Event to send when we reached the end.")]
		public FsmEvent finishedEvent;
		
		[ActionSection("")]
		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails")]
		public FsmEvent failureEvent;
		
		[ActionSection("Result")]
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The current index.")]
		public FsmInt currentIndex;
		
		
		// increment that index as we loop through item
		private int nextItemIndex = 0;
		
		
		public override void Reset()
		{
			startIndex = null;
			endIndex = 10;
			
			whileBool = null;
			
			loopEvent = null;
			finishedEvent = null;
			failureEvent = null;
	
			currentIndex = null;
			
		}
		
		
		public override void OnEnter()
		{
			if (whileBool.Value == false)
			{
				nextItemIndex = 0;
			}
			
			if (nextItemIndex == 0)
			{
				if (startIndex.Value>0)
				{
					nextItemIndex = startIndex.Value;
				}
				
			}
			
			
			DoGetNextItem();
			Finish();
			
			
			
		}
		
		void DoGetNextItem()
		{
			
			// no more children?
			// check first to avoid errors.

			if (nextItemIndex >= endIndex.Value)
			{
				nextItemIndex = 0;
				Fsm.Event(finishedEvent);
				return;
			}

			// get next item

			GetItemAtIndex();
			

			// no more items?
			// check a second time to avoid process lock and possible infinite loop if the action is called again.
			// Practically, this enabled calling again this state and it will start again iterating from the first child.
			if (nextItemIndex >= endIndex.Value)
			{
				nextItemIndex = 0;
				Fsm.Event(finishedEvent);
				return;
			}
			
			if (endIndex.Value>0 && nextItemIndex>= endIndex.Value)
			{
				nextItemIndex = 0;
				Fsm.Event(finishedEvent);
				return;
			}

			// iterate the next child
			nextItemIndex++;
			
			if (loopEvent != null)
			{
				Fsm.Event(loopEvent);
			}
		
		}
		public void GetItemAtIndex(){
			
			
			if (currentIndex.IsNone || whileBool.Value == false)
			{
				Fsm.Event(failureEvent);
				return;
			}
			currentIndex.Value = nextItemIndex;
			
			
		}	
			
		
		
		
	}
}
