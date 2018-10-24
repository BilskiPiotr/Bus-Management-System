﻿var soundObject = null;

function PlaySound(nazwa)
{
    _nazwa = nazwa;
    if (_nazwa != "")
    {
        if (soundObject != null)
        {
            document.body.removeChild(soundObject);
            soundObject.removed = true;
            soundObject = null;
        }
        soundObject = document.createElement("embed");
        soundObject.setAttribute("src", _nazwa);
        soundObject.setAttribute("hidden", true);
        soundObject.setAttribute("autostart", true);
        document.body.appendChild(soundObject);
    }
}