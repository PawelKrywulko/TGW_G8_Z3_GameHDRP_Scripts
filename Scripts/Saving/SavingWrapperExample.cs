﻿//using RPG.Saving;
//using System.Collections;
//using UnityEngine;

//namespace RPG.SceneManagement
//{
//    public class SavingWrapper : MonoBehaviour
//    {
//        [SerializeField] float fadeInTime = 2f;
//        const string defaultSaveFile = "save";

//        IEnumerator Start()
//        {
//            Fader fader = FindObjectOfType<Fader>();
//            fader.FadeOutImmediate();
//            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
//            yield return fader.FadeIn(fadeInTime);
//        }

//        private void Update()
//        {
//            if (Input.GetKeyDown(KeyCode.S))
//            {
//                Save();
//            }
//            if (Input.GetKeyDown(KeyCode.L))
//            {
//                Load();
//            }
//        }

//        public void Load()
//        {
//            GetComponent<SavingSystem>().Load(defaultSaveFile);
//        }

//        public void Save()
//        {
//            GetComponent<SavingSystem>().Save(defaultSaveFile);
//        }
//    }
//}