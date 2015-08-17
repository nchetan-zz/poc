package com.pradhee.chetann.updatetextviewinmainactivity;

import android.app.Activity;
import android.os.Bundle;
import android.os.Debug;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;

import java.util.Calendar;
import java.util.Date;

public class MainActivity extends Activity {

    private TextView mTextMessageTextView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        mTextMessageTextView = (TextView) findViewById(R.id.textMessage);
        mTextMessageTextView.setText("Hellooooo");
        setContentView(R.layout.activity_main);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    public void sendMessage(View view)
    {
        Calendar calendar = Calendar.getInstance();

        Date currentTime = calendar.getTime();
        mTextMessageTextView.setText(String.valueOf(currentTime));

        View mainActivity = findViewById(R.id.mainView);
        if (mainActivity == null)
        {
            Log.e("MainActivity", "Main activity is null.");
            return;
        }

        Log.d("MainActivity", "Main activity is null.");
        mainActivity.postInvalidate();
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
}
