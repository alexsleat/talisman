using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WebSocketSharp;
using Newtonsoft.Json;

public class RosBridgeClient : MonoBehaviour {

	private WebSocket ws;
	private string socketAddress = "ws://172.17.0.2:9090";
	private int reconnectUpdate = 1;
	private int ws_state = -1;

	private string notifyTopic = "/hri/notification";

	public string lastMsg = "";
	private string currentMsg = "";

// Use this for initialization
	void Start () {

		// Connect to the default address
		wsConnect(socketAddress);

	}

/// <summary>
/// wsConnect. websocket connection routine
/// </summary>
/// <param name="address">string address (and port, eg ws://127.0.0.1:9090)</param>
	public int wsConnect(string address){

		// Connect to the ROS Bridge WebSocket
		Debug.Log ("wsConnect: " + address);
		ws = new WebSocket (address);
		ws.ConnectAsync();

		// Wait a second before setting up advertisers and subscribers to ensure connection
		Invoke("wsSetup", 1f);

		return 1;
	}

/// <summary>
/// wsSetup. Set up advertisers and subscribers
/// </summary>
	public int wsSetup(){
		// Set up publishers
		//advertise(exampleTopic, "std_msgs/String");

		// Set up as subscribers
		subscribe(notifyTopic, "std_msgs/String", 0);

		return 1;
	}

/// <summary>
/// wsCheckState. Check the state websocket
/// </summary>
	public int wsCheckState(){

		if(ws.IsAlive == true){
			if(ws_state != 1){
				ws_state = 1;
				GameObject.Find("face").GetComponent<faceHandler>().SetFaceTrigger("Alive");
			}
		}
		else{
			if(ws_state != 0){
				GameObject.Find("face").GetComponent<faceHandler>().SetFaceTrigger("Dead");
				ws_state = 0;
			}
		}

		return ws_state;

	}

	void reconnect(){

		// If the next update is reached
		if(Time.time>=reconnectUpdate){
				Debug.Log ("websocket reconnection attempt ");
				Debug.Log(Time.time+">="+reconnectUpdate);
				// Change the next update (current second+1)
				reconnectUpdate=Mathf.FloorToInt(Time.time)+5;
				// Call connection function
				wsConnect(socketAddress);
			}
	}

// Update is called once per frame
	void Update () {

		// If the server connected, read topics
		if(wsCheckState() == 1){
			// Get the latest message:
			ws.OnMessage += (sender, e) =>
				 currentMsg = e.Data;

			// Check current message: ignore if it's not new.
			if(currentMsg != lastMsg){
				Debug.Log ("ROS says: " + currentMsg);
				// pass to manager
				subscribeManager(currentMsg);
				lastMsg = currentMsg;
			}
		}
		// If server isn't connected try to reconnect:
		else{
			reconnect();
		}

	}

/// <summary>
/// notifyRead. Decide what to do with the incoming message:
/// </summary>
/// <param name="input">string input </param>
	private void notifyRead(){

		// Do something

	}

/// <summary>
/// subscribeManager. Decide what to do with the incoming message:
/// </summary>
/// <param name="input">string input </param>
	private void subscribeManager(string input){

		Debug.Log("subscribeManager0: " + input);

		SubMsg o = JsonUtility.FromJson<SubMsg>(input);

		// Depending on the message, do accordingly
		string msg = o.msg.data;

		// If it's a notification it will start with Notify, and the second half
		// / of the message will be the message of the notification
		if(msg.StartsWith("Notify:")){

			msg = msg.Replace("Notify:", "");
			GameObject.Find("face").GetComponent<faceHandler>().SetNotification(msg);
			Debug.Log("Notification: " + msg);
		}
		else if(msg.StartsWith("Emote:")){

			msg = msg.Replace("Emote:", "");
			GameObject.Find("face").GetComponent<faceHandler>().SetFaceTrigger(msg);
			Debug.Log("Emote: " + msg);
		}
		// else if(msg.StartsWith("Weather:")){
		//
		// 	msg = msg.Replace("Weather:", "");
		// 	GameObject.Find("BackgroundWeather").GetComponent<backgroundHandler>().SetWeather(msg);
		// 	Debug.Log("Weather: " + msg);
		// }
		// Otherwise use the msg as an input for the animation
		else{
			GameObject.Find("face").GetComponent<faceHandler>().SetFaceTrigger(msg);
		}

	}

/// <summary>
/// subscribe. Set up a subscriber
/// </summary>
/// <param name="input">string input </param>
	public void subscribe(string topic, string type, int throttle_rate){

	// 	@TODO implement these:
  // (optional) "queue_length": <int>,
  // (optional) "fragment_size": <int>,
  // (optional) "compression": <string>

		Subscriber s = new Subscriber("subscribe", topic, type, throttle_rate);
		string jsonSub = JsonConvert.SerializeObject(s);
		ws.Send (jsonSub);

	}

/// <summary>
/// advertise. Set up an advertiser
/// </summary>
/// <param name="input">string input </param>
	public void advertise(string topic, string type){

		Advertiser a = new Advertiser("advertise", topic, type);
		string jsonAdv = JsonConvert.SerializeObject(a);
		ws.Send (jsonAdv);

	}

/// <summary>
/// Subscriber
/// </summary>
/// <param name="op">string data</param>
/// <param name="id">string data</param>
/// <param name="topic">string data</param>
/// <param name="type">string data</param>
/// <param name="throttle_rate">int data</param>
	[System.Serializable]
	private class Subscriber{
		public string op { get; set; }
		public string id { get; set; }
		public string topic { get; set; }
		public string type { get; set; }
		public int throttle_rate { get; set; }

		public Subscriber(string _receiver, string _topic, string _type, int _throttle_rate){
				op = _receiver;
				id = "unity_client";
				topic = _topic;
				type = _type;
				throttle_rate = _throttle_rate;
		}
	}

/// <summary>
/// Advertiser
/// </summary>
/// <param name="op">string data</param>
/// <param name="id">string data</param>
/// <param name="topic">string data</param>
/// <param name="msg">object data</param>
	[System.Serializable]
	private class Advertiser{
		public string op { get; set; }
		public string id { get; set; }
		public string topic { get; set; }
		public string type { get; set; }

		public Advertiser(string _receiver, string _topic, string _type){
				op = _receiver;
				id = "unity_client";
				topic = _topic;
				type = _type;
		}
	}

/// <summary>
/// Message
/// </summary>
/// <param name="op">string data</param>
/// <param name="id">string data</param>
/// <param name="topic">string data</param>
/// <param name="msg">object data</param>
	[System.Serializable]
	private class Message{

		public string op { get; set; }
		public string id { get; set; }
		public string topic { get; set; }
		public object msg { get; set; }

		public Message(string _receiver, string _topic, object _msg){
				op = _receiver;
				id = "unity_client";
				topic = _topic;
				msg = _msg;
		}
	}

/// <summary>
/// Message String Structure
/// </summary>
/// <param name="data">string data</param>
	[System.Serializable]
	public class MsgString{

		public string data { get; set; }

		public MsgString(string _data){
			data = _data;
		}

		public static MsgString CreateFromJSON(string jsonString)
		{
				return JsonUtility.FromJson<MsgString>(jsonString);
		}

	}

/// <summary>
/// Message: Pose2D Structure
/// </summary>
/// <param name="x">float x</param>
/// <param name="y">float y</param>
/// <param name="theta">float theta</param>
	[System.Serializable]
	public class MsgPose2D{

		public float x { get; set; }
		public float y { get; set; }
		public float theta { get; set; }

		public MsgPose2D(float _x, float _y, float _theta){
			x = _x;
			y = _y;
			theta = _theta;
		}
	}

/// <summary>
/// Subscripton Message
/// </summary>
/// <param name="topic">Topic Name</param>
/// <param name="msg">Message string(ROS)</param>
/// <param name="op">Op</param>
	[System.Serializable]
	public class SubMsg{

		public string topic;
		public ROString msg;
		public string op;

		public static SubMsg CreateFromJSON(string jsonString){
				return JsonUtility.FromJson<SubMsg>(jsonString);
		}

	}

/// <summary>
/// ROSString Message
/// </summary>
/// <param name="data">String data</param>
	[System.Serializable]
	public class ROString{

		public string data;

		public static ROString CreateFromJSON(string jsonString){
				return JsonUtility.FromJson<ROString>(jsonString);
		}

	}

}
