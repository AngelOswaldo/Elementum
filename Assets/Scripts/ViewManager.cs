using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager singleton;

    [SerializeField]
    private List<ViewController> views;

    private List<ViewController> instancedViews;

    public GameObject viewsLayer;

    private void Awake()
    {
        if(singleton)
        {
            Destroy(this.gameObject);
        }
        else
        {
            singleton = this;
        }
    }

    private T Instantiate<T>() where T: ViewController
    {
        var view = views.Find(view => view is T);
        
        if(view)
        {
            return Instantiate((T)view, viewsLayer.transform);
        }
        else
        {
            throw new Exception($"No item of type {typeof(T)}");
        }
    }

    public T Show<T>() where T : ViewController
    {
        var existent = instancedViews.Find(view => view is T);

        if(existent)
        {
            existent.Show();
            return existent as T;
        }
        else
        {
            var result = Instantiate<T>();
            instancedViews.Add(result);
            result.Show();
            return result;
        }
    }
}
