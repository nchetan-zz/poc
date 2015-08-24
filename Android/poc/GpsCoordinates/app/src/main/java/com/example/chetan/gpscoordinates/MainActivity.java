package com.example.chetan.gpscoordinates;

import android.location.Location;
import android.media.browse.MediaBrowser;
import android.media.browse.MediaBrowser.ConnectionCallback;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;

import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.location.LocationServices;

//
// IMP NOTE: To get coordinates of last location successfully, we need to start a google
// maps or another such application. Otherwise, lastLocation always returns null.
//
public class MainActivity extends AppCompatActivity implements GoogleApiClient.ConnectionCallbacks, GoogleApiClient.OnConnectionFailedListener {

    private TextView mLattitudeText;
    private TextView mLongitudeText;
    private GoogleApiClient mGoogleApiClient;
    private Location mLastLocation;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mLattitudeText = (TextView) findViewById(R.id.lattitude_text);
        mLongitudeText = (TextView) findViewById(R.id.longitude_text);

        buildGoogleApiClient();

    }

    @Override
    protected void onStart() {
        super.onStart();

        PlayServicesUtils.checkGooglePlaySevices(this);
        mGoogleApiClient.connect();
    }

    @Override
    protected void onStop() {
        super.onStop();
        if (mGoogleApiClient.isConnected()) {
            mGoogleApiClient.disconnect();
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        PlayServicesUtils.checkGooglePlaySevices(this);
        mGoogleApiClient.connect();
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
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

    public void refreshCoordinates(View view) {
        refreshCoordinates();
    }

    private void refreshCoordinates()
    {
        if (!mGoogleApiClient.isConnected())
        {
            Log.w("refreshCoordinates", "Api client is not connected.");
            if (!mGoogleApiClient.isConnecting())
            {
                Log.e("refreshCoordinates", "Api client is not connecting!!!");
            }
            return;
        }
        mLastLocation = LocationServices.FusedLocationApi.getLastLocation(mGoogleApiClient);
        double lastLocationLatitude = mLastLocation.getLatitude();
        double lastLocationLongitude = mLastLocation.getLongitude();

        mLattitudeText.setText(String.valueOf(lastLocationLatitude));
        mLongitudeText.setText(String.valueOf(lastLocationLongitude));
    }

    @Override
    public void onConnected(Bundle bundle) {
        Log.d("onConnected ", "Connected=" + String.valueOf(mGoogleApiClient.isConnected()));
        refreshCoordinates();
    }

    @Override
    public void onConnectionSuspended(int i) {
        Log.d("onConnectionSuspended ", "Connected=" + String.valueOf(mGoogleApiClient.isConnected()));
        mGoogleApiClient.connect();
    }

    @Override
    public void onConnectionFailed(ConnectionResult connectionResult) {
        Log.d("onConnectionFailed ", "Connected=" + String.valueOf(mGoogleApiClient.isConnected()));
    }

    protected synchronized void buildGoogleApiClient() {
        mGoogleApiClient = new GoogleApiClient.Builder(this)
                .addApi(LocationServices.API)
                .addConnectionCallbacks(this)
                .addOnConnectionFailedListener(this)
//                .addApi(LocationServices.API)
                .build();
    }
}
