using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderShow : MonoBehaviour
{
    public Renderer renderer_top;
    public Renderer renderer_left;
    public Renderer renderer_bot;
    public Renderer renderer_right;
    public Renderer renderer_mid;
    public void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (renderer_top.enabled == true)
            {
                enable(false);
            }
            else
            {
                enable(true);
            }
        }
    }

    public void enable(bool renderer_enable)
    {
        renderer_top.enabled = renderer_enable;
        renderer_left.enabled = renderer_enable;
        renderer_bot.enabled = renderer_enable;
        renderer_right.enabled = renderer_enable;
        renderer_mid.enabled = renderer_enable;
    }
}