using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinCollection : MonoBehaviour
{

	private int Coin = 0;

	public TextMeshProUGUI coinText;


	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Coin")
		{
			Coin++;
			coinText.text = "MonkeCoins : " + Coin.ToString() + " / 5";
			Debug.Log(Coin);
			Destroy(other.gameObject);
		}
		if (Coin == 5)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}

}
