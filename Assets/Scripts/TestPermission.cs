using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class TestPermission : MonoBehaviour
{
    [SerializeField] private Text storageStatusTxt;
    [SerializeField] private Text locationStatusTxt;

    private readonly string[] storagePermissions = new string[]
    {
        Permission.ExternalStorageRead,
        Permission.ExternalStorageWrite,
    };
    private readonly string[] locationPermissions = new string[]
    {
        Permission.FineLocation,
        Permission.CoarseLocation,
    };

    private bool StorageStatus
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                foreach (var permission in storagePermissions) if (!Permission.HasUserAuthorizedPermission(permission)) return false;
            }
            return true;
        }
    }

    private bool LocationStatus
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                foreach (var permission in locationPermissions) if (!Permission.HasUserAuthorizedPermission(permission)) return false;
            }
            return true;
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateStorage), 2, 1);
        InvokeRepeating(nameof(UpdateLocation), 2, 1);
    }

    public void UpdateStorage()
    {
        if (storageStatusTxt != null)
        {
            storageStatusTxt.text = "Storage: " + (StorageStatus ? "allowed" : "denied");
        }
    }

    public void UpdateLocation()
    {
        if (storageStatusTxt != null)
        {
            locationStatusTxt.text = "Location: " + (LocationStatus ? "allowed" : "denied");
        }
    }

    public void RequestPermission()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Permission.RequestUserPermissions(storagePermissions.Concat(locationPermissions).ToArray());
        }
    }
}
