using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomListing;

    private List<RoomListing> _listings=new List<RoomListing>();
    private RoomsCanvases _roomCanvases;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }
    public override void OnJoinedRoom()
    {
        _roomCanvases.CurrentRoomCanvas.Show();
        _content.DestroyChildren();
        _listings.Clear();
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //si il a été remove de room list.
            if(info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);  
                if(index!=-1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            //si il a été ajouté a room list.
            else
            { 
                int index = _listings.FindIndex(x=>x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    Debug.Log("ROOM AJOUTÉ A LISTE 1");
                    RoomListing listing = Instantiate(_roomListing, _content);
                    Debug.Log("ROOM AJOUTÉ A LISTE 2");


                    if (listing!= null)
                    {
                        Debug.Log("ROOM pas null");
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                }
                else
                {
                    //Pour Modifier la liste
                    //_listings[index].Faireqqch
                }
            }
        }
    }
}
