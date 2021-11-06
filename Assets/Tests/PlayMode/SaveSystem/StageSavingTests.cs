﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Fungus;

namespace SaveSystemTests
{
    public class StageSavingTests : SaveSysPlayModeTest
    {
        protected override string PathToScene => "Prefabs/StageSavingTests";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            GetPortraitsPrepped();
            RegisterStageAndPositions();
            RegisterCharacters();
        }

        protected virtual void GetPortraitsPrepped()
        {
            Flowchart prep = GameObject.Find("PrepPortraits").GetComponent<Flowchart>();
            prep.ExecuteBlock("Execute");
        }

        protected virtual void RegisterStageAndPositions()
        {
            stageForPortraits = GameObject.Find("TestStage").GetComponent<Stage>();
            onTheRight = GameObject.Find("TestRight").transform.name;
            onTheLeft = GameObject.Find("TestLeft").transform.name;
        }

        protected Stage stageForPortraits;
        protected string onTheRight, onTheLeft;

        protected virtual void RegisterCharacters()
        {
            sherlock = GameObject.Find("Sherlock Holmes").GetComponent<Character>();
            watson = GameObject.Find("John Watson").GetComponent<Character>();
        }

        protected Character sherlock, watson;

        [UnityTest]
        public virtual IEnumerator PortraitPositionsSaved()
        {
            yield return PostSetUp();

            bool yesTheyWereSaved = SherlockSavedAsOnTheRight() && WatsonSavedAsOnTheLeft();
            Assert.IsTrue(yesTheyWereSaved);
        }

        /// <summary>
        /// Every test will need this so they can be sure that the portraits are truly ready
        /// </summary>
        /// <returns></returns>
        protected IEnumerator PostSetUp()
        {
            yield return WaitForPortraitPrep();
            DimAsNeeded();
            savedPortraitStates = GetStatesOfAllPortraits();
            sherlockState = GetStateFor(sherlockName);
            watsonState = GetStateFor(watsonName);
            yield return new WaitForSeconds(1f); // Need to wait a little more for the dim effect
        }

        protected virtual IEnumerator WaitForPortraitPrep()
        {
            yield return new WaitForSeconds(portraitPrepTime);
        }

        protected float portraitPrepTime = 1.5f;

        protected virtual void DimAsNeeded()
        {
            stageForPortraits.SetDimmed(watson, true);
        }

        protected virtual IList<PortraitSaveState> GetStatesOfAllPortraits()
        {
            IList<Character> allChars = GameObject.FindObjectsOfType<Character>();
            IList<PortraitSaveState> states = new List<PortraitSaveState>();

            foreach (var character in allChars)
            {
                PortraitSaveState saveState = PortraitSaveState.From(character);
                states.Add(saveState);
            }

            return states;
        }

        protected IList<PortraitSaveState> savedPortraitStates = new List<PortraitSaveState>();

        protected PortraitSaveState sherlockState, watsonState;

        protected virtual bool SherlockSavedAsOnTheRight()
        {
            return sherlockState.PositionName == onTheRight;
        }

        protected string sherlockName = "Sherlock Holmes";

        protected virtual PortraitSaveState GetStateFor(string charName)
        {
            foreach (var savedState in savedPortraitStates)
                if (savedState.CharacterName == charName)
                    return savedState;

            return null;
        }

        protected virtual bool WatsonSavedAsOnTheLeft()
        {
            return watsonState.PositionName == onTheLeft;
        }

        protected string watsonName = "John Watson";


        protected virtual bool StateListsAreTheSame(IList<PortraitSaveState> firstStates, IList<PortraitSaveState> secondStates)
        {
            bool differentContentAmounts = firstStates.Count != secondStates.Count;
            if (differentContentAmounts)
                return false;

            int statesToCheck = firstStates.Count;
            for (int i = 0; i < statesToCheck; i++)
            {
                var firstStateEl = firstStates[i];
                var secondStateEl = secondStates[i];

                bool theyAreTheSame = firstStateEl.Equals(secondStateEl);
                if (!theyAreTheSame)
                    return false;
            }

            return true;
        }
        

        protected virtual IEnumerator HavePortraitsReady()
        {
            // Execute the block that prepares the portraits

            
            throw new System.NotImplementedException();
        }

        [UnityTest]
        public virtual IEnumerator PortraitsStageNamesSaved()
        {
            yield return PostSetUp();
            
            bool foundStateWithWrongStageName = false;

            foreach (var savedState in savedPortraitStates)
                if (savedState.StageName != testStageName)
                {
                    foundStateWithWrongStageName = true;
                    break;
                }

            bool thingsWereSavedProperly = !foundStateWithWrongStageName;
            Assert.IsTrue(thingsWereSavedProperly);

        }

        protected string testStageName = "TestStage";

        [UnityTest]
        public virtual IEnumerator PortraitsHaveCorrectCharacters()
        {
            yield return PostSetUp();

            bool onlyTwoStates = savedPortraitStates.Count == 2;
            bool oneForSherlock = GetStateFor(sherlockName) != null;
            bool oneForWatson = GetStateFor(watsonName) != null;

            bool thingsWentWell = onlyTwoStates && oneForSherlock && oneForWatson;
            Assert.IsTrue(thingsWentWell);
        }

        [UnityTest]
        public virtual IEnumerator PortraitDimStatesSaved()
        {
            yield return PostSetUp();

            bool sherlockNotDimmed = sherlockState.Dimmed == false;
            bool watsonDimmed = watsonState.Dimmed == true;

            bool savedCorrectly = sherlockNotDimmed && watsonDimmed;

            Assert.IsTrue(savedCorrectly);
        }

        [UnityTest]
        public virtual IEnumerator FacingDirectionsSaved()
        {
            yield return PostSetUp();

            bool sherlockFacingLeft = sherlockState.FacingDirection == FacingDirection.Left;
            bool watsonFacingRight = watsonState.FacingDirection == FacingDirection.Right;

            bool thingsAreAsIntended = sherlockFacingLeft && watsonFacingRight;
            Assert.IsTrue(thingsAreAsIntended);
        }

    }
}
