using UnityEngine;
using System.Collections;

public interface IDebuguable {
    /// <summary>
    /// Retourne la description de l'Objet
    /// </summary>
    /// <returns>Retourne la description de l'Objet</returns>
    string getDescription();

    /// <summary>
    /// Retourne le nom de l'Objet
    /// </summary>
    /// <returns>Retourne le nom de l'Objet</returns>
    string getName();

}
