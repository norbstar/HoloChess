using UnityEngine;

public interface IAudioComponent
{
    GameObject GetObject();
	void SyncVolume(float volume);
}