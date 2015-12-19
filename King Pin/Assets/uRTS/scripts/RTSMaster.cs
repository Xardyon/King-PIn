using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RTSMaster : MonoBehaviour {
 
 
    
    public List<GameObject> rtsUnitTypePrefabs = new List<GameObject>();
    
    public List<List<int>> numberOfUnitTypes = new List<List<int>>();
    public List<List<int>> unitTypeLocking = new List<List<int>>();
    
    public List<List<UnitPars>> unitsListByType = new List<List<UnitPars>>();
    
    
    
    public CameraSwitcher cameraSwitcher = null;
    public RTSCamera rtsCamera = null;
    public RPGCamera rpgCamera = null;
    
    public UIMaster uiMaster = null;
    
    public Terrain manualTerrain;
    public TerrainProperties terrainProperties;
    
    public MainUIFrame mainUIFrame = null;
    public ResourcePanel resourcePanel = null;
    public FullScreenButton fullScreenButton = null;
    public ActiveScreen activeScreen = null;
    public BottomBarInfo bottomBarInfo = null;
    public SelectedObjectInfo selectedObjectInfo = null;
    public BuildProgressNum buildProgressNum = null;
    public CancelSpawnButton cancelSpawnButton = null;
    public MineLabel mineLabel = null;
    public GroupingMenu groupingMenu = null;
    
//     public BuildDiplomacyMenu buildDiplomacyMenu = null;
    public DiplomacyMenu diplomacyMenu = null;
    public MovementButtons movementButtons = null;
    public BuildingButtons buildingButtons = null;
    public OptionsMenu optionsMenu = null;
    
    
    public Scores scores = null;
    public Cheats cheats = null;
    public Diplomacy diplomacy = null;
    public BuildMark buildMark = null;
    public Economy economy = null;
    public List<ResourcesCollection> resourcesCollection = new List<ResourcesCollection>();
    public CreateForest createForest = null;
    public BattleSystem battleSystem = null;
    public ResourcePoint resourcePoint = null;
    public CameraOperator cameraOperator = null;
    public SpritesManagerMaster spritesManagerMaster = null;
    public SelectionMark selectionMark = null;
    public GameOver gameOver;
    public UnitsMover unitsMover;
    public Formations formations;
    public UnitsGrouping unitsGrouping;
    public AgentMover agentMover;
    public FPSCount fpsCount;
    public FPSSelfRegulator fpsSelfRegulator;
    public ScreenSizeChangeActions screenSizeChangeActions;
    public ScreenSizeChangeActionsEditor screenSizeChangeActionsEditor;
    public GameObject projector;
    public GameObject mainCanvas;
    
    public List<GameObject> nationCenters = new List<GameObject>();
    public List<NationPars> nationPars = new List<NationPars>();
    public List<NationAI> nationAIs = new List<NationAI>();
    public List<BattleAI> battleAIs = new List<BattleAI>();
    public List<WondererAI> wondererAIs = new List<WondererAI>();
    
    
    public List<UnitPars> allUnits = new List<UnitPars>();
    
    
//    public List<int> nationMilitaryUnits = new List<int>();
    
    
    
	// Use this for initialization
	void Awake () {
//	    diplomacy = GameObject.Find("Terrain").GetComponent<Diplomacy>();
	    int NN = diplomacy.numberNations;
	    
	    numberOfUnitTypes = new List<List<int>>();
	    unitTypeLocking = new List<List<int>>();
	    
	    for(int i=0;i<NN;i++){
	        numberOfUnitTypes.Add(new List<int>());
	        unitTypeLocking.Add(new List<int>());
	
			for(int j=0;j<rtsUnitTypePrefabs.Count;j++){
				numberOfUnitTypes[i].Add(0);
				unitTypeLocking[i].Add(0);
			}
		}
		
		for(int i=0;i<rtsUnitTypePrefabs.Count;i++){
			unitsListByType.Add(new List<UnitPars>());
		}
//		manualTerrain.GetComponent<TerrainCollider>().terrainData = manualTerrain.terrainData;
	}
	
	
	
	public void DestroyUnit(UnitPars goPars){
	//	UnitPars goPars = go.GetComponent<UnitPars>();
		
		battleSystem.RemoveUnitFromBS(goPars);
		cameraOperator.DeselectObject(goPars);
		nationAIs[goPars.nation].UnsetUnit(goPars);
		wondererAIs[goPars.nation].RemoveUnit(goPars);
		resourcesCollection[goPars.nation].RemoveFromResourcesCollection(goPars);
		unitsListByType[goPars.rtsUnitId].Remove(goPars);
		unitsMover.CompleteMovent(goPars);
		numberOfUnitTypes[goPars.nation][goPars.rtsUnitId] = numberOfUnitTypes[goPars.nation][goPars.rtsUnitId] - 1;
		unitsGrouping.RemoveUnitFromGroup(goPars);
		formations.RemoveUnitFromFormation(goPars);
		Destroy(goPars.gameObject);
	}
	
	
	
	
	
	
}
