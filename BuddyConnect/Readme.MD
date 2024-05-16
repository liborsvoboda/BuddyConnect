
# Maui Technology Description    

[Popis Technologie](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/menu-bar?view=net-maui-8.0)     

[Maui Objekty](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/tableview?view=net-maui-8.0)   


---    

# Maui Samples   
[Příklady Aplikací](https://github.com/dotnet/maui-samples/tree/main/8.0/Fundamentals/Shell)  
[Pěkné Menu](https://user-images.githubusercontent.com/5375137/165635140-8e10db78-94a6-490c-bc85-dc87dc1e1b7d.png)   
[Tvurce Rozpis Maui](https://github.com/dotnet/maui/tree/1b05e12bed02bdcfdfe1d705fde83237da22b5c6/src/Controls/samples/Controls.Sample/wwwroot)   
[Dev POPISY](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/tableview?view=net-maui-8.0)    

[Příklady Menu](https://github.com/dotnet/maui-samples/blob/main/8.0/Fundamentals/Shell/Xaminals/AppShell.xaml)  
[ladeni kompatibility](https://github.com/dotnet/maui-samples/blob/main/Upgrading/CustomRenderer/MultiProject/Entry/Android/MyEntryRenderer.cs)   
[Dokumentace Menu](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/flyout?view=net-maui-8.0)   


[addon UI](https://enisn-projects.io/docs/en/uranium/latest/Getting-Started#themes-available)   

[MENU](https://stackoverflow.com/questions/76314109/net-maui-example-hamburger-menu-non-shell)   

https://github.com/dotnet/maui/issues/17965
[Flyout](https://www.c-sharpcorner.com/article/flyoutpage-in-net-maui/)  
[DropDown](https://medium.com/devexpress-technical/net-maui-3-combobox-dropdown-alternatives-for-user-friendly-item-selection-within-a-mobile-app-879435f342da)   

---    

# Installation Instructions

```bash  
dotnet workload install maccatalyst  
dotnet workload install ios   
dotnet workload install android   
``` 

---    

# Developer Help

[GRAPHIC MAUI DEVELOPER HELP](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/pages?view=net-maui-8.0)   
[TIPY A VARIABILITY ZDROJE](https://github.com/jsuarezruiz/awesome-dotnet-maui?tab=readme-ov-file#samples)    
[Vlastní Styly](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/styles/css?view=net-maui-7.0)   
[Prakticke ukazky](https://vladislavantonyuk.github.io/articles/Setting-a-cursor-for-.NET-MAUI-VisualElement/#google_vignette)   


---    

[bluetooth windows](https://learn.microsoft.com/en-us/answers/questions/1314706/how-to-check-system-bluetooth-is-on-off-(platform))    
[bluetooth android](https://stackoverflow.com/questions/76858093/how-to-activate-deactivate-bluetooth-in-net-maui)   
[BT Connect](https://www.c-sharpcorner.com/article/implementation-of-the-bluetooth-connectivity-using-net-maui/)  
[bluetooth plugin](https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le)   
[alternativa](https://github.com/inthehand/32feet)    


---   
# BT Tool Online

https://vyvoj.ctgroup.cz/dvbt/

# Centralni Logika Aplikace   
Centralni Logiky slouzi pro jednotne nastaveni a chovani aplikace.   
Pokud je tedy potřeba nejaka zmena nastavi se prave 1x 
v dane logice a projevi se vsude v aplikaci kde je pouzita.   

1. Aplikace Používá Theming  - Změna Vzhledu   
 **/Themes/DarkTheme.xaml**   
 **/Themes/LightTheme.xaml** 

V nich se nastavuji Definice pto Typy Objektu a jejich Barvy   
Vybrany styl se pak nastavi vyberem v menu pro celou aplikaci     

2) Definice Vlastnosti Objektu: Nadpisy, Okno, tlacitko,....
  App.xaml   
 
 Zde jsou definovany Centralni vlastnosti: 
- Font, Velikost, zaobleni, odsazeni,  atd..  
  NE BARVY - ty musi byt na objektu, pouze typ TRANSPARENT    

**!!! POZOR BARVY THEME DEFINOVA NA UROVNI FORMULARE !!!**   
**!!! NASTAVENIM STYLU OBJKU NEDOJDE KE ZMENE!!!**  

3) Jazyky vsechny Texty krome vlastnich DAT jsou definovany ve slovnicich
  Resources/Languages/cs, de, en

veskere Texty je nutne definovat zde a pro aplikaci pouzivat    
volani odkazu na slovnik    

4) Hodnoty a Nastaveni - DATABAZE  
   **Database.cs**   
 veskere Datove a promenlive Hodnoty jsou ukladany do Lokalni SQLLite DB   

To zarucuje plnou kontrolu nad veskerymi Daty    
Data jsou Typu:   
-  **Setting**: a ma tyto vlastnosti:
   Nastaveni vychozich Hodnot, Statrup nacteni, Zmeny a Ukladani
   Provedeni konkretni akce: (zmena barev, textu, atd..)   

-  **Data**: Ty maji svou definici vzdy s ID, Timestamp, Data
   a maji Save/Load/Delte: pro zobrazeni, zapis, mazani, atd..


5) Sablona Formulare: Volba Menu   
   **TemplateListPage.xaml**   
Ma prednastavene chovani, vzhledy a styly pro snadnou aplikaci    
Novych Voleb v Menu    


6) Stav - Informace Aplikace    
   **AppSettings**   
aby data byla dostupna vzdy a vsude pridavat je to deto Denifice    
tim se zaruci spravnost nastaveni pri preskakovani v menu
   
Obsahuje tyto informace:  
-  Nastaveni
-  stavy hardware
-  Databazi
-  Datove kolekce

7) Centralni Logiky a Definice Struktury aplikace  
Ta je rozdelena podle povahy a pouziti na

-  Datove Denice: GlobalClasses.cs   
-  Database: DatabaseModel.cs  [First,Create/Open/Connect] 
-  DB Operace: Controllers/Model.cs [DB, SAVE/LOAD/UPD/DEL,atd..]
-  Startup Operace: StartupControls.cs [Nacteni a nastaveni pri startu]  
-  Konstanty:  GlobalConstants.cs [app konstanty, HW,..]  
-  Agendy: GlobalControls/Agenda.cs [poznamky,novinky,log,..]   
-  Funkce: GlobalFunctions.cs   
-  Interface:  GlobalServices.cs
-  Platforms: definice pro IOS/ANdroid/WIN  
-  Resources: Obrazky,Styly,Barvy,Fonty  


**!!!POZOR pri preskakovani nedochazi k opetovne iniciaci formulare!!!**   

---    
  
# Dynamická Pravidla Programování   

1. nastaveni Dynamické Barvy   
- noteItem.SetDynamicResource(TextCell.TextColorProperty, "PrimaryTextColor");   
- noteItem.SetDynamicResource(TextCell.DetailColorProperty, "SecondaryTextColor");     
    
2. Nastavení Dynamického Slovníku   
- some.Text = AppResources.TranslatedWord   
   
3. Všechny slovníkové Popisy XAML   
- vložit do TranslatePageObjects()  
  pro dynamický překlad online při změně  
- je překládán zaroven s provedenim LoadStartupData   
  při každé změně JAzyk/Theme   
   

4. Try Catch Zaznamenávání chyb na pozadí do DB   
vše vkládat do TRY/CATCH s kódem:    

```cs   
catch (Exception ex) { 
    await DetectedErrorListController.SaveDetectedErrorList(new DetectedErrorList() { Message = SystemFunctions.GetSystemErrMessage(ex) });
}
```   


---     
 
# Přínosy  Serveru pro App    

- ukladani chyb na serer pro OK vyvoj
- Novinky ze Serveru
- stranky ze serveru
- 

# Možnosti Aplikace: Centrální a Globální LOGIKA  

- aplikace je multiplatformní TZN. 1 VÝVOJ PRO VŠECHNY windows,ios,android,mac  
- aplikace stojí na Databázi:  sběr dat, logování:
- - možnosti detailních analýz 
  - sběru požadavků
  - vyhodncení a nabídka akcí uživatelů dle prováděných úkonů a jejich oblíbenosti
  - vyhodnocování používání a jejich optimalizace
  - modení vývoj a Online nasazení změn
  - sběru informací o výkonu, integritě software a následné ladění
- Aplikace využívá multimediální rozhraní
- - umí vše od vytváření libovolných mediálních prezentací např z obrázků, 
   Fotek pod vodou idělat slideshow prezentaci na Facebook, vosílat živá videa
  - komunikovat s okolními a spřátelenými zařízeními, servery, atd.

- Automaticky multijazyčná: slovník se dá nastavit na serverru a centrálně jej stravovat
- libovolná správa obsahu aPLIKace: menu, možnosti, fukncionality lze dynamicky
- nastavit tak aby se z 1 administrační aplikace dala měnit aplikace na mobilech všech užvatelů na celé planetě
  pouhým nastavením v administraci na serveru, které si mobily stáhnou ze serveru na pozadí
  a sami se aktualizují

- aplikace umí vnitřně zobrazovat webvové stránky: tzn 1 WEB = centrální systém 
  všechny mobily uvidí změny na webu ihned po otevření bez nutnosti nějakých updatů, atd

- Dynamické menu: lze nastavit stahování ze serveru tak, že aplikace nabídne možnosti
  stažené ze serveru a sama si je doplní. Není nutný Vývoj, nová menu se vygenerují dle nastavení

- Centrální globální správe všech aplikací, verzí, jednotek

TAK by měl být bussines v dbnešní dynamické době nastaven
proto aby to takto fungovalo musí naslouchat i p. Bartoš a upravit tak konfiguraci
místo tvrzerní že jsem konfliktní se na to dívat z tohoto pohledu

  
 
