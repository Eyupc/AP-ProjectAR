using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageManager : MonoBehaviour
{

    public GameObject settingsCanvas, homeCanvas, menuCanvas;

    private string guide, language, sound, close, settings;
    
    private string welcome, intro, bioTitle, bio, tipTitle, tip, journeyTitle, journey, readyTitle, ready;
    private List<string> stops = new List<string>();
    
    private string menuTitle, show;
    List<List<string>> menuOptions = new List<List<string>>();
    
    private static bool languageChanged = false;

    public static void LanguageChanged() {
        languageChanged = true;
    }

    private void ChangeLanguage() {
        languageChanged = false;
        if (UserSystemManager.Language == Language.Dutch) {
            guide = "Gids";
            language = "Taal";
            sound = "Geluid";
            close = "Sluiten";
            settings = "Instellingen";

            welcome = "Marhaba! Ontdek Syrië in Antwerpen"; 
            intro = "Ik ben Abdel en ik kijk ernaar uit om je mee te nemen op een reis door mijn twee werelden: Syrië en Antwerpen. Samen ontdekken we de rijke Syrische cultuur hier in het hart van België."; 
            bioTitle = "Een beetje over mezelf"; 
            bio = "Het grootste deel van mijn leven bracht ik door in Syrië, maar sinds enkele jaren woon ik in Antwerpen. Nu wil ik graag mijn ervaringen en de schoonheid van de Syrische cultuur met jou delen, terwijl we samen door deze prachtige stad wandelen."; 
            tipTitle = "Een tip voor de beste beleving"; 
            tip = "Gebruik oortjes of een koptelefoon voor de beste ervaring. Zo voelt het echt alsof we samen op pad zijn!"; 
            journeyTitle = "Onze reis samen"; 
            journey = "Laat me je meenemen naar enkele bijzondere plekken"; 
            readyTitle = "Klaar om te beginnen?"; 
            ready = "Gebruik de kaart om onze eerste stop bij het Stadspark te vinden. Ik kijk ernaar uit om mijn Syrië met jou te delen!";
            stops.Clear();
            stops.Add("Een stukje poëzie");
            stops.Add("Een reis naast het station van Damascus");
            stops.Add("Proef de smaken van Syrië");
            stops.Add("De skyline van Antwerpen en Aleppo");
            stops.Add("Ontdek de zeep van Aleppo");

            show = "Toon menu";
            menuTitle = "Menu kaart";
            menuOptions.Clear();
            List<string> option1 = new List<string>();
            option1.Add("Kibbeh");
            option1.Add("Aleppo");
            option1.Add("Bulgur, vlees, kruiden; gefrituurd, gebakken, of rauw gerecht");
            menuOptions.Add(option1);
            List<string> option2 = new List<string>();
            option2.Add("Kebab B’il Karaz");
            option2.Add("Aleppo");
            option2.Add("Gegrild vlees, kersen, zoet-zure saus");
            menuOptions.Add(option2);
            List<string> option3 = new List<string>();
            option3.Add("Fattoush");
            option3.Add("Damascus, Aleppo");
            option3.Add("Frisse salade, groenten, pita, sumak, citroendressing");
            menuOptions.Add(option3);
            List<string> option4 = new List<string>();
            option4.Add("Shakriyeh");
            option4.Add("Damascus");
            option4.Add("Syrische yoghurtstoofpot, lamsvlees, knoflook, kruiden, geserveerd met rijst");
            menuOptions.Add(option4);
        } else {
            guide = "الدليل";
            language = "اللغة";
            sound = "الصوت";
            close = "إغلاق";
            settings = "الإعدادات";

            welcome = "مرحبا اكتشف سوريا في أنتويرب"; 
            intro = "أنا عبدل وأتطلع إلى اصطحابكم في رحلة عبر عالميّ: سوريا وأنتويرب. سنكتشف معاً الثقافة السورية الغنية هنا في قلب بلجيكا."; 
            bioTitle = "نبذة عن نفسي";
            bio = "قضيت معظم حياتي في سوريا، ولكن منذ بضع سنوات وأنا أعيش في أنتويرب. وأود الآن أن أشارككم تجاربي وجمال الثقافة السورية بينما نسير معاً في هذه المدينة الجميلة."; 
            tipTitle = "نصيحة للحصول على أفضل تجربة"; 
            tip = "استخدم سماعات الأذن أو سماعات الرأس للحصول على أفضل تجربة. بهذه الطريقة، ستشعر حقاً وكأننا على الطريق معاً!"; 
            journeyTitle = "رحلتنا معاً"; 
            journey = "دعني آخذك إلى بعض الأماكن المميزة"; 
            readyTitle = "هل أنت جاهز للبدء؟";
            ready = "استخدم الخريطة للعثور على محطتنا الأولى في حديقة المدينة. أتطلع إلى مشاركة سوريا معكم!";
            stops.Clear();
            stops.Add("مقطوعة شعرية");
            stops.Add("رحلة بمحاذاة محطة دمشق");
            stops.Add("تذوق نكهات سوريا");
            stops.Add("أفق مدينة أنتويرب وحلب");
            stops.Add("اكتشف صابون حلب");

            show = "عرض القائمة";
            menuTitle = "بطاقة القائمة";
            menuOptions.Clear();
            List<string> option1 = new List<string>();
            option1.Add("كبة");
            option1.Add("حلب");
            option1.Add("البرغل واللحم والتوابل؛ طبق مقلي أو مخبوز أو نيء");
            menuOptions.Add(option1);
            List<string> option2 = new List<string>();
            option2.Add("كباب بالكرز");
            option2.Add("حلب");
            option2.Add("اللحم المشوي والكرز والصلصة الحلوة والحامضة");
            menuOptions.Add(option2);
            List<string> option3 = new List<string>();
            option3.Add("فتوش");
            option3.Add("دمشق, حلب");
            option3.Add("سلطة طازجة، خضروات، خضروات، بيتا، سماق، صلصة الليمون");
            menuOptions.Add(option3);
            List<string> option4 = new List<string>();
            option4.Add("الشاكرية");
            option4.Add("دمشق");
            option4.Add("مرق الزبادي السوري مع لحم الضأن والثوم والتوابل واللبن السوري يقدم مع الأرز");
            menuOptions.Add(option4);
        }

        FindChildByName<TMP_Text>(settingsCanvas.transform, "GuideText").text = guide;
        FindChildByName<TMP_Text>(settingsCanvas.transform, "LanguageText").text = language;
        FindChildByName<TMP_Text>(settingsCanvas.transform, "SoundText").text = sound;
        FindChildByName<TMP_Text>(settingsCanvas.transform, "CloseButtonText").text = close;
        FindChildByName<TMP_Text>(settingsCanvas.transform, "Title").text = settings;

        homeCanvas.SetActive(true);
        FindChildByName<TMP_Text>(homeCanvas.transform, "WelcomeTitle").text = welcome;
        FindChildByName<TMP_Text>(homeCanvas.transform, "IntroText").text = intro;
        FindChildByName<TMP_Text>(homeCanvas.transform, "BioTitle").text = bioTitle;
        FindChildByName<TMP_Text>(homeCanvas.transform, "BioText").text = bio;
        FindChildByName<TMP_Text>(homeCanvas.transform, "TipTitle").text = tipTitle;
        FindChildByName<TMP_Text>(homeCanvas.transform, "TipText").text = tip;
        FindChildByName<TMP_Text>(homeCanvas.transform, "JourneyTitle").text = journeyTitle;
        FindChildByName<TMP_Text>(homeCanvas.transform, "JourneyText").text = journey;
        FindChildByName<TMP_Text>(homeCanvas.transform, "ReadyTitle").text = readyTitle;
        FindChildByName<TMP_Text>(homeCanvas.transform, "ReadyText").text = ready;
        FindChildByName<TMP_Text>(homeCanvas.transform, "Stop1Text").text = stops[0];
        FindChildByName<TMP_Text>(homeCanvas.transform, "Stop2Text").text = stops[1];
        FindChildByName<TMP_Text>(homeCanvas.transform, "Stop3Text").text = stops[2];
        FindChildByName<TMP_Text>(homeCanvas.transform, "Stop4Text").text = stops[3];
        FindChildByName<TMP_Text>(homeCanvas.transform, "Stop5Text").text = stops[4];
        homeCanvas.SetActive(false);

        menuCanvas.SetActive(true);
        FindChildByName<TMP_Text>(menuCanvas.transform, "ShowText").text = show;
        FindChildByName<TMP_Text>(menuCanvas.transform, "MenuTitle").text = menuTitle;
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option1Title").text = menuOptions[0][0];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option2Title").text = menuOptions[1][0];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option3Title").text = menuOptions[2][0];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option4Title").text = menuOptions[3][0];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option1Location").text = menuOptions[0][1];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option2Location").text = menuOptions[1][1];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option3Location").text = menuOptions[2][1];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option4Location").text = menuOptions[3][1];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option1Description").text = menuOptions[0][2];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option2Description").text = menuOptions[1][2];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option3Description").text = menuOptions[2][2];
        FindChildByName<TMP_Text>(menuCanvas.transform, "Option4Description").text = menuOptions[3][2];
        menuCanvas.SetActive(false);
    }

    T FindChildByName<T>(Transform parent, string name) where T : Component
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child.GetComponent<T>();
            }
            T result = FindChildByName<T>(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    void Update() 
    {
        if (languageChanged) {
            ChangeLanguage();
        }
    }
}
