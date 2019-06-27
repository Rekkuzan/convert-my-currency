package com.rekkuzan.datepicker

import android.app.DatePickerDialog
import com.unity3d.player.UnityPlayer
import java.util.*


object DatePickerUnity {

    @JvmStatic fun StartDatePicker(unityObjectName: String, unityMethodName: String) {

        val c = Calendar.getInstance()
        val yearNow = c.get(Calendar.YEAR)
        val monthNow = c.get(Calendar.MONTH)
        val dayNow = c.get(Calendar.DAY_OF_MONTH)

        val dpd = DatePickerDialog(
            UnityPlayer.currentActivity, DatePickerDialog.OnDateSetListener
            { _, year, monthOfYear, dayOfMonth ->
                val format = "%04d-%02d-%02d";
                val message = format.format(year, monthOfYear + 1, dayOfMonth);
                UnityPlayer.UnitySendMessage(unityObjectName, unityMethodName, message)
            }, yearNow, monthNow, dayNow
        )

        dpd.show()
    }
}