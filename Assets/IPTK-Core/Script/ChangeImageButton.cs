using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeImageButton : Interactable
{
    public ImageDisplay imageDisplay;  // 引用 ImageDisplay 脚本，用于切换图片
    public bool isNextButton;          // 如果是 true，表示此按钮用于下一张图片，否则为上一张图片

    void Start()
    {
        // 可以在这里做一些初始化
    }

    public override void Interact()
    {
        base.Interact();

        // 根据按钮的类型，调用 ImageDisplay 中的图片切换函数
        if (imageDisplay != null)
        {
            if (isNextButton)
            {
                Debug.Log("Next button clicked");
                imageDisplay.NextImage();  // 调用下一张图片的方法
            }
            else
            {
                Debug.Log("Previous button clicked");
                imageDisplay.PreviousImage();  // 调用上一张图片的方法
            }
        }
    }
}
