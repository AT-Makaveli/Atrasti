import { firebase } from '@react-native-firebase/messaging';
import { GoogleServices } from "./GoogleServices";
import { DeviceEventEmitter, EmitterSubscription } from "react-native";
import AuthRequestEvent from "../utils/Events/Auth/AuthRequestEvent";
import { setFcmToken } from "../Api/UserAPI";
import ChatReceiveEvent from "../utils/Events/Chat/ChatReceiveEvent";

const googleServices: GoogleServices = require('../../assets/json/google-services.json');

let onAuthenticateSubscription: EmitterSubscription;

export function setupCloudMethods() {
    onAuthenticateSubscription = DeviceEventEmitter.addListener(AuthRequestEvent.EVENT_TYPE, onAuthenticate);
}

export function onAuthenticate() {
    registerFirebaseApp().then(app => {
        registerToken(app);

        firebase.messaging().onNotificationOpenedApp(remoteMessage => {
            console.log(
                'Notification caused app to open from background state:',
                remoteMessage.notification,
            );
            //navigation.navigate(remoteMessage.data.type);
        });

        // Check whether an initial notification is available
        firebase.messaging()
            .getInitialNotification()
            .then(remoteMessage => {
                if(remoteMessage) {
                    console.log(
                        'Notification caused app to open from quit state:',
                        remoteMessage.notification,
                    );
                    //setInitialRoute(remoteMessage.data.type); // e.g. "Settings"
                }
                //setLoading(false);
            });

        firebase.messaging().onMessage(message => {
            const msgData = message.data;
            if (msgData === undefined) return;

            const header = msgData.header;
            if (header === undefined) return;

            if (header === "CHAT_MESSAGE")  {
                DeviceEventEmitter.emit(ChatReceiveEvent.EVENT_TYPE, new ChatReceiveEvent(msgData));
            }
        });
    });
}

export function unSubscribeCloudMethods() {
    onAuthenticateSubscription?.remove();
}

export function registerFirebaseApp() {
    return new Promise<any>((async (resolve, reject) => {
        const options = {
            appId: googleServices.client[0].client_info.mobilesdk_app_id,
            projectId: googleServices.project_info.project_id,
            storageBucket: googleServices.project_info.storage_bucket,
            apiKey: 'AIzaSyCJlUDxSAN6akLqluPLoEZm_Dsyp1PBMPo',
            databaseURL: '',
            messagingSenderId: ''
        };

        if(!firebase.apps.length) {
            firebase.initializeApp(options)
                .then(app => {
                    resolve(app as any);
                }).catch(error => {
                reject(error);
            });
        } else {
            resolve(firebase.app());
        }
    }));
}

export function registerToken(app: any) {
    firebase.messaging()
        .getToken()
        .then((token: string) => {
            setFcmToken(token).then(result => {

            }).catch(errors => {
                console.log(errors);
            });
        });

    // If using other push notification providers (ie Amazon SNS, etc)
    // you may need to get the APNs token instead for iOS:
    // if(Platform.OS == 'ios') { messaging().getAPNSToken().then(token => { return saveTokenToDatabase(token); }); }

    // Listen to whether the token changes
    return firebase.messaging().onTokenRefresh((token: string) => {
        setFcmToken(token).then(result => {

        }).catch(errors => {
            console.log(errors);
        });
    });
}
