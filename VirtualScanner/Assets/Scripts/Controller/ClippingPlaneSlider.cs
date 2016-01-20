using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClippingPlaneSlider : MonoBehaviour 
{
    public Slider slider;
    public Transform clippingPlane;
    private Vector3 initPlanePosition;

    void Start()
    {
        this.initPlanePosition = clippingPlane.transform.position;
    }

    public void SlideClippingPlane()
    {
        this.clippingPlane.transform.position = this.initPlanePosition + this.slider.value * this.clippingPlane.transform.up.normalized;
    }
}
