using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;

public class SaveManager : Singleton<SaveManager>
{
	[Header("Parameters")]
	[SerializeField]
	private string identifier = "gamedata";

	[SerializeField]
	private string password = "hello&";

	[SerializeField]
	private SaveGamePath savePath = SaveGamePath.DataPath;

	// mode: new SaveGameXmlSerializer(), new SaveGameJsonSerializer(), new SaveGameBinarySerializer()
	private ISaveGameSerializer activeSerializer = new SaveGameJsonSerializer();

	[SerializeField] private SaveData data = new SaveData();

	private IEnumerable<IBackupableEntity> backupables;



	public void Save()
	{
		this.backupables = FindObjectsOfType<MonoBehaviour>().OfType<IBackupableEntity>();

		foreach(IBackupableEntity ent in this.backupables) {
			ent.OnSave(this.data);
		}

		SaveGame.Save<SaveData>(this.identifier,
								this.data,
								false,
								this.password,
								this.activeSerializer,
								null,
								SaveGame.DefaultEncoding,
								this.savePath);

		// Debug.Log("SAVED !");
	}

	public void Load()
	{
		this.data = SaveGame.Load<SaveData>(this.identifier,
											new SaveData(),
											false,
											this.password,
											this.activeSerializer,
											null,
											SaveGame.DefaultEncoding,
											this.savePath);

		this.backupables = FindObjectsOfType<MonoBehaviour>().OfType<IBackupableEntity>();

		foreach(IBackupableEntity ent in backupables) {
			ent.OnLoad(this.data);
		}

		// Debug.Log("LOADED !");
	}
}
