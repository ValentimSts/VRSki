
namespace Assets.Scripts.Controller
{
    public class ControllerAction
    {
        private Action action;
        private Side side;

        private ButtonType buttonType;
        private int button;


        public ButtonType ButtonType => buttonType;
        public int ButtonValue => button;

        
        public ControllerAction(Action action, Side side)
        {
            this.action = action;
            this.side = side;

            buttonType = GetButtonTypeAssociatedWithAction(action);
            button = (int)GetButtonAssociatedWithAction(action, side);
        }


        private ButtonType GetButtonTypeAssociatedWithAction(Action action)
        {
            return action switch
            {
                Action.Respawn => ButtonType.Button,
                Action.ShootRifle => ButtonType.Trigger,
                Action.EndObjectInteraction => ButtonType.Trigger,

                // This should never happen.
                _ => ButtonType.None,
            };
        }

        private Button GetButtonAssociatedWithAction(Action action, Side side)
        {
            return side switch
            {
                Side.Left => GetButtonAssociatedWithActionLeft(action),
                Side.Right => GetButtonAssociatedWithActionLeft(action),

                // This should never happen.
                _ => Button.None,
            };
        }

        private Button GetButtonAssociatedWithActionLeft(Action action)
        {
            return action switch
            {
                Action.Respawn => Button.X,
                Action.ShootRifle => Button.LeftIndexTrigger,
                Action.EndObjectInteraction => Button.LeftHandTrigger,

                // This should never happen.
                _ => Button.None,
            };
        }

        private Button GetButtonAssociatedWithActionRight(Action action)
        {
            return action switch
            {
                Action.Respawn => Button.A,
                Action.ShootRifle => Button.RightIndexTrigger,
                Action.EndObjectInteraction => Button.RightHandTrigger,

                // This should never happen.
                _ => Button.None,
            };
        }
    }
}