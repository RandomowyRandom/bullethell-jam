using UnityEngine;

[CreateAssetMenu(menuName = "MapInfo")]
public class MapInfo : ScriptableObject
{
    [Header("Main Path")]
    [SerializeField] [Range(1, 4)] private int _mainPathsNumber;
    [SerializeField] private int _minCellInMainPath;
    [SerializeField] private int _maxCellInMainPath;
    [Header("Additional paths")]
    [SerializeField] private int _additionalPathsNumber;
    [SerializeField] private int _minCellInAdditionalPath;
    [SerializeField] private int _maxCellInAdditionalPath;

    public int MainPathsNumber => _mainPathsNumber;

    public int MINCellInMainPath => _minCellInMainPath;

    public int MAXCellInMainPath => _maxCellInMainPath;

    public int AdditionalPathsNumber => _additionalPathsNumber;

    public int MINCellInAdditionalPath => _minCellInAdditionalPath;

    public int MAXCellInAdditionalPath => _maxCellInAdditionalPath;
}