import { CardStyleInterpolators, createStackNavigator } from "@react-navigation/stack";
import React from "react";
import { HeaderTitleProps } from "@react-navigation/elements";
import { Text } from "react-native";
import MyProfileView from "./MyProfileView";
import ManageProfileView from "./Views/Company/ManageProfileView";
import BottomDrawer from "./Views/Company/BottomDrawer";
import UploadProductView from "./Views/Company/UploadProductView";
import ManageAgentView from "./Views/Agent/ManageAgentView";
import AgentBottomDrawer from "./Views/Agent/AgentBottomDrawer";
import ProfileView from "./ProfileView";
import ChatView from "../Chat/ChatView";

const Stack = createStackNavigator();

export interface ProfileProps {
    initialRoute: string | undefined
}

export default class ProfileStack extends React.Component<ProfileProps, any> {

    render() {
        return (
            <Stack.Navigator initialRouteName={"ProfileView"} screenOptions={{
                headerShown: false,
                gestureDirection: "horizontal",
                cardStyleInterpolator: CardStyleInterpolators.forHorizontalIOS,
            }}>
                <Stack.Screen name="ProfileView" component={MyProfileView}/>
                <Stack.Screen
                    name="BottomDrawer"
                    component={BottomDrawer}
                    options={{
                        presentation: 'transparentModal',
                        cardOverlayEnabled: true,
                        cardStyleInterpolator: CardStyleInterpolators.forVerticalIOS,
                    }}
                />
                <Stack.Screen
                    name="AgentBottomDrawer"
                    component={AgentBottomDrawer}
                    options={{
                        presentation: 'transparentModal',
                        cardOverlayEnabled: true,
                        cardStyleInterpolator: CardStyleInterpolators.forVerticalIOS,
                    }}
                />
                <Stack.Screen name="ManageProfileView" component={ManageProfileView} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Manage Profile</Text>
                    }
                }}/>
                <Stack.Screen name="UploadProductView" component={UploadProductView} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Upload Product</Text>
                    }
                }}/>
                <Stack.Screen name="ManageAgentView" component={ManageAgentView} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Manage site</Text>
                    },
                }}/>
                <Stack.Screen name="ProfileUserChat" component={ChatView} options={{
                    headerShown: true,
                    headerStyle: {
                        backgroundColor: '#000000',
                    },
                    headerTitle: (props: HeaderTitleProps): any => {
                        return <Text style={{
                            color: '#FFFFFF',
                            fontSize: 16,
                            fontWeight: "bold"
                        }}>Chat user</Text>
                    }
                }} />
            </Stack.Navigator>
        );
    }
}
