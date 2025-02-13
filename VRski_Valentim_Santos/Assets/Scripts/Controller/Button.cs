
namespace Assets.Scripts.Controller
{
    public enum Button
    {
        None = OVRInput.Button.None,

        // Left controller buttons.
        X = OVRInput.Button.One,
        Y = OVRInput.Button.Two,
        LeftIndexTrigger = OVRInput.Axis1D.PrimaryIndexTrigger,
        LeftHandTrigger = OVRInput.Axis1D.PrimaryHandTrigger,

        // Right controller buttons.
        A = OVRInput.Button.Three,
        B = OVRInput.Button.Four,
        RightIndexTrigger = OVRInput.Axis1D.SecondaryIndexTrigger,
        RightHandTrigger = OVRInput.Axis1D.SecondaryHandTrigger
    }
}