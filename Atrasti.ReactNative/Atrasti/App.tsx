import * as React from "react";
import { DefaultTheme, NavigationContainer } from "@react-navigation/native";
import AuthStack from "./src/AuthStack";
import AuthenticatedStack from "./src/AuthenticatedStack";
import { DeviceEventEmitter, EmitterSubscription } from "react-native";
import AuthRequestEvent from "./src/utils/Events/Auth/AuthRequestEvent";
import AuthSignOutEvent from "./src/utils/Events/Auth/AuthSignOutEvent";
import { setupCloudMethods, unSubscribeCloudMethods } from "./src/FirebaseCloud/FirebaseCloudMethods";
import { loadCountries } from "./src/Api/CountryAPI";

const MyTheme = {
    ...DefaultTheme,
    colors: {
        ...DefaultTheme.colors,
        card: '#343a40'
    },
};

interface AppState {
    authenticated: boolean;
}

class App extends React.Component<any, AppState> {

    private _onAuthRequest: EmitterSubscription | undefined;
    private _onAuthSignOutRequest: EmitterSubscription | undefined;

    constructor(props: any) {
        super(props);

        this.state = {
            authenticated: false
        }
    }

    async componentDidMount() {
        this._onAuthRequest = DeviceEventEmitter.addListener(AuthRequestEvent.EVENT_TYPE, this.onAuthEvent.bind(this));
        this._onAuthSignOutRequest = DeviceEventEmitter.addListener(AuthSignOutEvent.EVENT_TYPE, this.onAuthSignOutEvent.bind(this));
        setupCloudMethods();
        await loadCountries();
    }

    componentWillUnmount() {
        unSubscribeCloudMethods();
        this._onAuthRequest?.remove();
        this._onAuthSignOutRequest?.remove();
    }

    onAuthEvent() {
        this.setState({
            authenticated: true
        })
    }

    onAuthSignOutEvent() {
        this.setState({
            authenticated: false
        })
    }

    render() {
        return (<NavigationContainer theme={MyTheme}>
            {!this.state.authenticated ? <AuthStack/> : <AuthenticatedStack/>}
        </NavigationContainer>);
    }

}

export default App;
