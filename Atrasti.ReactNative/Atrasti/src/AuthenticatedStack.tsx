import React from "react";
import { CardStyleInterpolators, createStackNavigator } from "@react-navigation/stack";
import ProfileView from "./Profile/ProfileView";
import AuthTabs from "./AuthTabs";

const Stack = createStackNavigator();

export default class AuthenticatedStack extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
    }

    render() {
        return (
            <Stack.Navigator initialRouteName={'MainTabs'} id={'AuthenticatedStack'} screenOptions={{
                headerShown: false,
                gestureDirection: "horizontal",
                cardStyleInterpolator: CardStyleInterpolators.forHorizontalIOS
            }}>
                <Stack.Screen name={'MainTabs'} component={AuthTabs} />
                <Stack.Screen name={'OtherProfile'} component={ProfileView} />
            </Stack.Navigator>
        );
    }
}
