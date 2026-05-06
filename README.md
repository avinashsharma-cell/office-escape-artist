# Slack & Thrive - Corporate Idle Game

A humorous 2D side-scrolling idle/clicker game set in a corporate office environment.

## Game Overview

Player controls a customizable office employee whose sole objective is to survive the workday by doing as little work as possible whilst maintaining their sanity, health, and reputation.

## Visual Style

- Art Style: Flat 2D illustration with cartoonish, light-hearted aesthetic
- Perspective: Side-scrolling view with visible office floors
- Animation: Idle animations, walk cycles, reaction animations
- UI: Corporate-themed HUD with fake "Employee Dashboard" aesthetic

## Office Layout

- Floor 5: C-Suite / Executive Level (Final unlock)
- Floor 4: Finance & Accounts
- Floor 3: Marketing & Social Media
- Floor 2: IT & Tech Support
- Floor 1: General Operations (Start)
- Basement: Cafeteria & Break Room (Always accessible)

## Core Mechanics

- 2D Movement with arrow keys/WASD
- Manager Detection System with cone of vision
- Interactive office objects tied to Slack Points
- Mini-games: Alt-Tab Dash, Excuse Delivery, Sneak Past Security, Bluff Builder, Inbox Avalanche
- Sanity Meter (styled as "Productivity" to fool viewers)
- Suspicion Meter
- Floor progression system

## Files

- PlayerController.cs - Player movement and actions
- ManagerAI.cs - Manager NPC AI and detection
- InteractableObject.cs - Base class for office objects
- OfficeObjects.cs - Specific office object implementations
- NPCCharacters.cs - All NPC character classes
- MiniGames.cs - All mini-game implementations
- GameManager.cs - Central game state management
- UIManager.cs - HUD and UI management
- CharacterCustomization.cs - Player character customization
- SaveSystem.cs - Save/load functionality
