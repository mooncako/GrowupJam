using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayHost : MonoBehaviour
{
    public async void StartHost()
    {
        try
        {
            // Create an allocation for max players (host + clients)
            Allocation alloc = await RelayService.Instance.CreateAllocationAsync(maxConnections: 3);

            // Get a join code to share with friends
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
            EventHub.Instance.OnJoinCodePublished.Invoke(joinCode);
            Debug.Log($"Relay Join Code: {joinCode}");

            // Configure Unity Transport to use Relay
            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetRelayServerData(
                alloc.RelayServer.IpV4,
                (ushort)alloc.RelayServer.Port,
                alloc.AllocationIdBytes,
                alloc.Key,
                alloc.ConnectionData
            );

            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }
}
