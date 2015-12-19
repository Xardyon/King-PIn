using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RTSCompiler : MonoBehaviour {

	public List<GameObject> rtsUnitTypePrefabs = new List<GameObject>();
	[HideInInspector] public List<GameObject> goList = new List<GameObject>();
	
	[HideInInspector] public RTSMaster rtsm;	
	[HideInInspector] public Diplomacy diplomacy;
	[HideInInspector] public BattleSystem battleSystem;
	[HideInInspector] public Scores scores;
	[HideInInspector] public SelectionMark selectionMark;
	[HideInInspector] public Economy economy;
	[HideInInspector] public List<ResourcesCollection> resourcesCollection = new List<ResourcesCollection>();
	[HideInInspector] public FPSCount fpsCount;
	[HideInInspector] public FPSSelfRegulator fpsSelfRegulator;
	[HideInInspector] public SpritesManagerMaster spritesManagerMaster;
	[HideInInspector] public CameraOperator cameraOperator;
	[HideInInspector] public ResourcePoint resourcePoint;
	[HideInInspector] public CreateForest createForest;
	[HideInInspector] public BuildMark buildMark;
	[HideInInspector] public TerrainProperties terrainProperties;
	
	[HideInInspector] public List<string> nationNames;
//	[HideInInspector] public List<string> unusedNationNames;
// 	[HideInInspector] public BuildDiplomacyMenu buildDiplomacyMenu;
	
	
	public GameObject projectorPrefab;
	public List<GameObject> resourcePointPrefab = new List<GameObject>();
	
	private List<GameObject> nationCenters = new List<GameObject>();
	private List<SpawnPoint> nationCentersSpawns = new List<SpawnPoint>();
	private List<NationAI> nationAIs = new List<NationAI>();
	
	private List<Vector3> centresPosList = new List<Vector3>();

	
	public int numberNations = 4;
	
	
	
	[HideInInspector] public bool rtsCompiled;
	
	public Terrain manualTerrain;

	public void Compile() {
		FillCentresPosList();
		AddNationNames();
		SetRTSM();
		if(rtsCompiled == false){
		    SetEventSystem();
		    
		    SetTerrainProperties();
		    
			SetDiplomacy();
			SetBattleSystem();
			SetScores();
			SetCheats();
			SetSelectionMark();
	        SetEconomy();
	   //     SetResourcesCollection();
	        SetPerformanceManager();
	        SetSpritesManagerMaster();
			SetProjector();
			SetCameraOperator();
			SetResourcePoint();
			SetCreateForest();
			SetNationCenters();
			SetBuildMark();
			SetRTSCamera();
			SetGameOver();
			SetUnitsMover();
			SetFormations();
			SetUnitsGrouping();
			SetAgentMover();
			
			SetMainCanvas();
			SetDirectionalLight();
			
			SetSecondLinks();
			
			rtsm.uiMaster.LoadIcons();
			
			rtsm.mainUIFrame.Build();
			rtsm.resourcePanel.Build();
			rtsm.fullScreenButton.Build();
			
			rtsm.activeScreen.Build();
			rtsm.bottomBarInfo.Build();
			rtsm.selectedObjectInfo.Build();
			rtsm.buildProgressNum.Build();
			rtsm.cancelSpawnButton.Build();
			rtsm.mineLabel.Build();
			rtsm.groupingMenu.Build();
			
// 			buildDiplomacyMenu.Build();
			rtsm.diplomacyMenu.Build();
			rtsm.movementButtons.Build();
			rtsm.buildingButtons.Build();
			
			rtsm.optionsMenu.Build();
		}
		
		
	//	print("aaa");
		
	}
	
	
	
	
	public void CleanUp(){
	    terrainProperties.Clean();
	    for(int i=0;i<goList.Count;i++){
			DestroyImmediate(goList[i]);
		}
		goList.Clear();
		nationCenters.Clear();
		nationCentersSpawns.Clear();
		resourcesCollection.Clear();
        nationAIs.Clear();
	}
	
	
	
	// RTS Master
	void SetRTSM(){

        GameObject rtsmGo = null;
        
        
		
		GameObjectExistanceCheck(ref rtsCompiled, ref rtsmGo, "RTS Master");
		

	    if(rtsCompiled == false){
			rtsmGo = new GameObject("RTS Master");
			goList.Add(rtsmGo);
			rtsm = rtsmGo.AddComponent<RTSMaster>();
			rtsm.rtsUnitTypePrefabs = rtsUnitTypePrefabs;
			rtsm.manualTerrain = manualTerrain;
			
			manualTerrain.GetComponent<TerrainCollider>().terrainData = manualTerrain.terrainData;
			
		}
	}
    
    void SetTerrainProperties(){
    	GameObject go = new GameObject("TerrainProperties");
		goList.Add(go);
		terrainProperties = go.AddComponent<TerrainProperties>();
		rtsm.terrainProperties = terrainProperties;
		terrainProperties.rtsm = rtsm;
		terrainProperties.GetTerrainProperties();
    }
    
    
    
    
    // Diplomacy
    void SetDiplomacy(){
    	GameObject diplomacyGo = new GameObject("Diplomacy");
		goList.Add(diplomacyGo);
		diplomacy = diplomacyGo.AddComponent<Diplomacy>();
		rtsm.diplomacy = diplomacy;
		diplomacy.numberNations = numberNations;
    }

    // Battle System
    void SetBattleSystem(){
    	GameObject battleSystemGo = new GameObject("BattleSystem");
		goList.Add(battleSystemGo);
		battleSystem = battleSystemGo.AddComponent<BattleSystem>();
		rtsm.battleSystem = battleSystem;
		battleSystem.attackDistance = 300f;

    }
    
    // Scores
    void SetScores(){
    	GameObject scoresGo = new GameObject("Scores");
		goList.Add(scoresGo);
		scores = scoresGo.AddComponent<Scores>();
		rtsm.scores = scores;

    }
    
    // Cheats
    void SetCheats(){
    	GameObject cheatsGo = new GameObject("Cheats");
		goList.Add(cheatsGo);
//		scores = scoresGo.AddComponent<Cheats>();
		rtsm.cheats = cheatsGo.AddComponent<Cheats>();
		rtsm.cheats.rtsm = rtsm;

    }
    
    // SelectionMark
    void SetSelectionMark(){
    	GameObject selectionMarkGo = new GameObject("SelectionMark");
		goList.Add(selectionMarkGo);
		selectionMark = selectionMarkGo.AddComponent<SelectionMark>();
		rtsm.selectionMark = selectionMark;
		selectionMark.BuildSelectionMarkTextures();
		selectionMark.BuildHealthBarTextures();
    }

    // Economy
    void SetEconomy(){
    	GameObject economyGo = new GameObject("Economy");
		goList.Add(economyGo);
		economy = economyGo.AddComponent<Economy>();
		rtsm.economy = economy;

    }
//     
//     // ResourcesCollection
//     void SetResourcesCollection(){
//     	GameObject resourcesCollectionGo = new GameObject("ResourcesCollection");
// 		goList.Add(resourcesCollectionGo);
// 		resourcesCollection.Add(resourcesCollectionGo.AddComponent<ResourcesCollection>());
// 		rtsm.resourcesCollection.Add(resourcesCollection[0]);
// 
//     }

    // PerformanceManager
    void SetPerformanceManager(){
    	GameObject performanceManagerGo = new GameObject("PerformanceManager");
		goList.Add(performanceManagerGo);
		fpsCount = performanceManagerGo.AddComponent<FPSCount>();
		rtsm.fpsCount = fpsCount;
		fpsSelfRegulator = performanceManagerGo.AddComponent<FPSSelfRegulator>();
		rtsm.fpsSelfRegulator = fpsSelfRegulator;
		

    }

    // SpritesManagerMaster
    void SetSpritesManagerMaster(){
    	GameObject spritesManagerMasterGo = new GameObject("SpritesManagerMaster");
		goList.Add(spritesManagerMasterGo);
		spritesManagerMaster = spritesManagerMasterGo.AddComponent<SpritesManagerMaster>();
		rtsm.spritesManagerMaster = spritesManagerMaster;

    }

    // Projector
    void SetProjector(){
    	GameObject projectorGo = (GameObject)Instantiate(projectorPrefab);
    //    GameObject projectorGo = new GameObject("n");
    //    projectorGo = projectorPrefab;
    	projectorGo.name = "Projector";
		goList.Add(projectorGo);
		rtsm.projector = projectorGo;
		Projector proj = projectorGo.GetComponent<Projector>();
		proj.orthographic = true;
		proj.orthographicSize = 2f;
		

    }

    // CameraOperator
    void SetCameraOperator(){
    	GameObject cameraOperatorGo = new GameObject("CameraOperator");
		goList.Add(cameraOperatorGo);
		cameraOperator = cameraOperatorGo.AddComponent<CameraOperator>();
		rtsm.cameraOperator = cameraOperator;

    }

    // ResourcePoint
    void SetResourcePoint(){
    	GameObject resourcePointGo = new GameObject("ResourcePoint");
		goList.Add(resourcePointGo);
		resourcePoint = resourcePointGo.AddComponent<ResourcePoint>();
		rtsm.resourcePoint = resourcePoint;
		resourcePoint.ironPrefab = resourcePointPrefab[0];
		resourcePoint.goldPrefab = resourcePointPrefab[1];

    }

    // CameraOperator
    void SetCreateForest(){
    	GameObject createForestGo = new GameObject("CreateForest");
		goList.Add(createForestGo);
		createForest = createForestGo.AddComponent<CreateForest>();
		createForestGo.AddComponent<CreateForestEdit>();
		rtsm.createForest = createForest;
		createForest.rtsm = rtsm;
		createForest.SetDefaultSettings();
		createForest.CalculateAllTreePositions();

    }

    void SetNationCenters(){
		
		for(int i=0; i<numberNations; i++){
			nationCenters.Add(new GameObject("NationCentre"+(i).ToString()));
			goList.Add(nationCenters[i]);
			rtsm.nationCenters.Add(nationCenters[i]);
			nationCentersSpawns.Add(nationCenters[i].AddComponent<SpawnPoint>());
			nationCentersSpawns[i].nation = i;
			resourcesCollection.Add(nationCenters[i].AddComponent<ResourcesCollection>());
			rtsm.resourcesCollection.Add(resourcesCollection[i]);
			nationAIs.Add(nationCenters[i].AddComponent<NationAI>());
			rtsm.nationAIs.Add(nationAIs[i]);
			rtsm.nationPars.Add(nationCenters[i].AddComponent<NationPars>());
			
			rtsm.battleAIs.Add(nationCenters[i].AddComponent<BattleAI>());
			rtsm.battleAIs[i].rtsm = rtsm;
			rtsm.battleAIs[i].nation = i;
			
			rtsm.wondererAIs.Add(nationCenters[i].AddComponent<WondererAI>());
			rtsm.wondererAIs[i].rtsm = rtsm;
			rtsm.wondererAIs[i].nation = i;
			
			
			rtsm.nationAIs[i].rtsm = rtsm;
			rtsm.nationAIs[i].nation = i;
//			rtsm.nationAIs[i].enabled = false;
			rtsm.nationAIs[i].FillLists();
			
			rtsm.nationPars[i].nation = i;
			
			for(int j=0; j<numberNations; j++){
			    rtsm.nationPars[i].rNations.Add(0f);
				rtsm.nationPars[i].neighboursDistanceFrac.Add(0f);
			}
			
			
			
		}
		
		
		
// 		nationCentersSpawns[0].model = rtsUnitTypePrefabs[0];
// 		nationCentersSpawns[0].numberOfObjects = 1;
// 
// 		nationCentersSpawns[1].model = rtsUnitTypePrefabs[1];
// 		nationCentersSpawns[1].numberOfObjects = 0;
// 	//	nationCentersSpawns[1].size = 100f;
// 
// 		nationCentersSpawns[2].model = rtsUnitTypePrefabs[1];
// 		nationCentersSpawns[2].numberOfObjects = 0;
// 	//	nationCentersSpawns[2].size = 80f;
// 
// 		nationCentersSpawns[3].model = rtsUnitTypePrefabs[1];
// 		nationCentersSpawns[3].numberOfObjects = 3;
// 	//	nationCentersSpawns[3].size = 80f;
// 		
		
		for(int i=0; i<numberNations; i++){
		    nationCentersSpawns[i].model = rtsUnitTypePrefabs[0];
			nationCentersSpawns[i].numberOfObjects = 1;
			nationCenters[i].transform.position = centresPosList[i];
		}
		
// 		nationCenters[0].transform.position = new Vector3(585f, 6.5f, 225f);
// 		nationCenters[1].transform.position = new Vector3(826, 6f, 353);
// 	//	nationCenters[1].transform.position = new Vector3(1427f, 6f, 306f);
// 		nationCenters[2].transform.position = new Vector3(1220f, 6.5f, 654f);
// 	//	nationCenters[2].transform.position = new Vector3(1276f, 6.5f, 1258f);
// 		nationCenters[3].transform.position = new Vector3(515f, 6.5f, 1258f);
		
		for(int i=0; i<numberNations; i++){
			nationAIs[i].nationName = nationNames[i];
			nationAIs[i].nation = i;
		}
		
// 		nationAIs[0].nationName = "Thomas";
// 		nationAIs[0].nation = 0;
// 		
// 	//	nationAIs[1] = nationCenters[1].GetComponent<NationAI>();
// 	//	rtsm.nationAIs[1] = nationAIs[1];
// 		nationAIs[1].nationName = "Eli";
// 		nationAIs[1].nation = 1;
// //		nationAIs[1].enabled = true;
// 		
// 	//	nationAIs[2] = nationCenters[2].GetComponent<NationAI>();
// 	//	rtsm.nationAIs[2] = nationAIs[2];
// 		nationAIs[2].nationName = "Vseslav";
// 		nationAIs[2].nation = 2;
// //		nationAIs[2].enabled = true;
// 
// 		nationAIs[3].nationName = "Omer";
// 		nationAIs[3].nation = 3;
// //		nationAIs[3].enabled = true;
// 


    	
    }
    
    
    
    void AddNationNames(){
    	nationNames = new List<string>();
    	
    	nationNames.Add("Thomas");
    	nationNames.Add("Eli");
    	nationNames.Add("Vseslav");
    	nationNames.Add("Omer");
    	nationNames.Add("Teore");
    	nationNames.Add("Rapomi");
    }
    
    
    
    
    // BuildMark
    void SetBuildMark(){
    	GameObject buildMarkGo = new GameObject("BuildMark");
		goList.Add(buildMarkGo);
		buildMark = buildMarkGo.AddComponent<BuildMark>();
		rtsm.buildMark = buildMark;
		buildMark.enabled = false;

    }
    
    // RTSCamera
    void SetRTSCamera(){
    	if(Camera.main.gameObject.GetComponent<RTSCamera>() == null){
    		rtsm.rtsCamera = Camera.main.gameObject.AddComponent<RTSCamera>();
    	}
    	else{
    		rtsm.rtsCamera = Camera.main.gameObject.GetComponent<RTSCamera>();
    	}
    	
    	if(Camera.main.gameObject.GetComponent<CameraSwitcher>() == null){
    		rtsm.cameraSwitcher = Camera.main.gameObject.AddComponent<CameraSwitcher>();
    		rtsm.cameraSwitcher.rtsm = rtsm;
    	}
    	else{
    		rtsm.cameraSwitcher = Camera.main.gameObject.GetComponent<CameraSwitcher>();
    		rtsm.cameraSwitcher.rtsm = rtsm;
    	}
    	if(Camera.main.gameObject.GetComponent<RPGCamera>() == null){
    		rtsm.rpgCamera = Camera.main.gameObject.AddComponent<RPGCamera>();
    		rtsm.rpgCamera.rtsm = rtsm;
    		rtsm.rpgCamera.enabled = false;
    	}
    	else{
    		rtsm.rpgCamera = Camera.main.gameObject.GetComponent<RPGCamera>();
    		rtsm.rpgCamera.rtsm = rtsm;
    		rtsm.rpgCamera.enabled = false;
    	}
    	
    }
    
    void SetGameOver(){
    	GameObject gameOverGo = new GameObject("GameOver");
    	goList.Add(gameOverGo);
    	rtsm.gameOver = gameOverGo.AddComponent<GameOver>();
    	rtsm.gameOver.rtsm = rtsm;
    }
    
    void SetUnitsMover(){
    	GameObject unitsMoverGo = new GameObject("UnitsMover");
    	goList.Add(unitsMoverGo);
    	rtsm.unitsMover = unitsMoverGo.AddComponent<UnitsMover>();
    	rtsm.unitsMover.rtsm = rtsm;
    }
    
    void SetFormations(){
    	GameObject formationsGo = new GameObject("Formations");
    	goList.Add(formationsGo);
    	rtsm.formations = formationsGo.AddComponent<Formations>();
    	rtsm.formations.rtsm = rtsm;
    }
    
    void SetUnitsGrouping(){
    	GameObject go = new GameObject("UnitsGrouping");
    	goList.Add(go);
    	rtsm.unitsGrouping = go.AddComponent<UnitsGrouping>();
    	rtsm.unitsGrouping.rtsm = rtsm;
    }
    
    void SetAgentMover(){
    	GameObject agentMoverGo = new GameObject("AgentMover");
    	goList.Add(agentMoverGo);
    	rtsm.agentMover = agentMoverGo.AddComponent<AgentMover>();
    	rtsm.agentMover.rtsm = rtsm;
    }
    
   // MainCanvas 
    void SetMainCanvas(){
    	GameObject mainCanvasGo = new GameObject("MainCanvas");
    	rtsm.mainCanvas = mainCanvasGo;
		goList.Add(mainCanvasGo);
		Canvas canvas = mainCanvasGo.AddComponent<Canvas>();
		mainCanvasGo.AddComponent<CanvasScaler>();
		mainCanvasGo.AddComponent<GraphicRaycaster>();
		
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		mainCanvasGo.layer = LayerMask.NameToLayer("UI");
		
		rtsm.uiMaster = mainCanvasGo.AddComponent<UIMaster>();
		rtsm.uiMaster.mainCanvas = mainCanvasGo;
		
		rtsm.mainUIFrame = mainCanvasGo.AddComponent<MainUIFrame>();
		rtsm.resourcePanel = mainCanvasGo.AddComponent<ResourcePanel>();
		rtsm.fullScreenButton = mainCanvasGo.AddComponent<FullScreenButton>();
		
		rtsm.activeScreen = mainCanvasGo.AddComponent<ActiveScreen>();
		
		rtsm.bottomBarInfo = mainCanvasGo.AddComponent<BottomBarInfo>();
		rtsm.selectedObjectInfo = mainCanvasGo.AddComponent<SelectedObjectInfo>();
		rtsm.buildProgressNum = mainCanvasGo.AddComponent<BuildProgressNum>();
		rtsm.cancelSpawnButton = mainCanvasGo.AddComponent<CancelSpawnButton>();
		rtsm.mineLabel = mainCanvasGo.AddComponent<MineLabel>();
		rtsm.groupingMenu = mainCanvasGo.AddComponent<GroupingMenu>();
		
// 		buildDiplomacyMenu = mainCanvasGo.AddComponent<BuildDiplomacyMenu>();
// 		rtsm.buildDiplomacyMenu = buildDiplomacyMenu;
		
		
		rtsm.diplomacyMenu = mainCanvasGo.AddComponent<DiplomacyMenu>();
		rtsm.movementButtons = mainCanvasGo.AddComponent<MovementButtons>();
		rtsm.buildingButtons = mainCanvasGo.AddComponent<BuildingButtons>();
		rtsm.optionsMenu = mainCanvasGo.AddComponent<OptionsMenu>();
		
		rtsm.screenSizeChangeActions = mainCanvasGo.AddComponent<ScreenSizeChangeActions>();
		
		
		rtsm.screenSizeChangeActionsEditor = mainCanvasGo.AddComponent<ScreenSizeChangeActionsEditor>();
		rtsm.screenSizeChangeActionsEditor.ssca = rtsm.screenSizeChangeActions;
		
		rtsm.screenSizeChangeActions.prevScreenWidth = Screen.width;
		rtsm.screenSizeChangeActions.prevScreenHeight = Screen.height;
		
    
   }
   
   
   // EventSystem
   void SetEventSystem(){
   		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
		bool isEventSystemPresent = false;
		foreach(GameObject go in allObjects){
			if(go.GetComponent<EventSystem>()==true){
				isEventSystemPresent = true;
			}
		}
		if(isEventSystemPresent == false){
			GameObject eventSystem = new GameObject("EventSystem");
			goList.Add(eventSystem);
			eventSystem.AddComponent<EventSystem>();
			eventSystem.AddComponent<StandaloneInputModule>();
			eventSystem.AddComponent<TouchInputModule>();
		}

   }  
   
   
   void SetDirectionalLight(){
   		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
   		bool isLightPresent = false;
   		foreach(GameObject go in allObjects){
			if(go.GetComponent<Light>()==true){
				isLightPresent = true;
			}
		}
		if(isLightPresent == false){
			GameObject lightGo = new GameObject("Directional light");
			lightGo.transform.position = new Vector3(-500f,500f,-500f);
			lightGo.transform.eulerAngles = new Vector3(50f,330f,0f);
			goList.Add(lightGo);
			Light light = lightGo.AddComponent<Light>();
			light.type = LightType.Directional;
			light.intensity = 1.5f;
			light.shadows = LightShadows.Soft;
			light.shadowStrength = 0.7f;
			
		}
		
   } 
    
   
    
    
    
    void SetSecondLinks(){
    	diplomacy.rtsm = rtsm;
    	battleSystem.rtsm = rtsm;
    	scores.rtsm = rtsm;
    	selectionMark.rtsm = rtsm;
        economy.rtsm = rtsm;
        
        fpsCount.rtsm = rtsm;
        fpsSelfRegulator.rtsm = rtsm;
        spritesManagerMaster.rtsm = rtsm;
        cameraOperator.rtsm = rtsm;
        resourcePoint.rtsm = rtsm;
        createForest.rtsm = rtsm;
        buildMark.rtsm = rtsm;
        
        rtsm.uiMaster.rtsm = rtsm;
        
        rtsm.mainUIFrame.rtsm = rtsm;
        rtsm.resourcePanel.rtsm = rtsm;
        rtsm.fullScreenButton.rtsm = rtsm;
        
        rtsm.rtsCamera.rtsm = rtsm;
        rtsm.activeScreen.rtsm = rtsm;
        
        rtsm.bottomBarInfo.rtsm = rtsm;
        rtsm.selectedObjectInfo.rtsm = rtsm;
        rtsm.buildProgressNum.rtsm = rtsm;
        rtsm.cancelSpawnButton.rtsm = rtsm;
        rtsm.mineLabel.rtsm = rtsm;
        rtsm.groupingMenu.rtsm = rtsm;
        
//        buildDiplomacyMenu.rtsm = rtsm;
        rtsm.diplomacyMenu.rtsm = rtsm;
        rtsm.movementButtons.rtsm = rtsm;
        rtsm.buildingButtons.rtsm = rtsm;
        rtsm.optionsMenu.rtsm = rtsm;
        
        rtsm.screenSizeChangeActions.rtsm = rtsm;
        
        for(int i=0; i<numberNations; i++){
        	resourcesCollection[i].rtsm = rtsm;
        	rtsm.nationPars[i].rtsm = rtsm;
        }
    }
   
	
	
	
	void GameObjectExistanceCheck(ref bool isFound, ref GameObject go, string name){
		GameObject[] sceneList = UnityEngine.Object.FindObjectsOfType<GameObject>();
		isFound = false;
		
		for(int i=0;i<sceneList.Length;i++){
			if(sceneList[i].name == name){
			   go = sceneList[i];
			   isFound = true;
			}
		}
		
	}
	
	
	
	
// 	bool ScriptExitanceCheck(string scriptName){
// 		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
// 		bool isScriptPresent = false;
// 		foreach(GameObject go in allObjects){
// 			if(go.GetComponent(scriptName)==true){
// 				isScriptPresent = true;
// 			}
// 		}
//         return isScriptPresent;
// 	}
// 	
	
	
	void Start(){
	
	}
	
	
	void FillCentresPosList(){
		centresPosList.Clear();
		centresPosList.Add(new Vector3(585f, 6.5f, 225f));
		centresPosList.Add(new Vector3(826f, 6f, 353f));
		centresPosList.Add(new Vector3(1220f, 6.5f, 654f));
		centresPosList.Add(new Vector3(515f, 6.5f, 1258f));
		centresPosList.Add(new Vector3(1427f, 6f, 306f));
		centresPosList.Add(new Vector3(1276f, 6.5f, 1258f));
	}
	
	
	
}
