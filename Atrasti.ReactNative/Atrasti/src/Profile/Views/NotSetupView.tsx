import React from "react";
import { Image, StyleSheet, Text, View } from "react-native";
import { NotExistProfile } from "../../utils/StaticImageUrls";
import AtrastiButton from "../../Shared/AtrastiButton";
import { NavigationScreenProp } from "react-navigation";
import { UserType_Mod } from "../../Api/Models/DbModels/UserType_Mod";

export interface NotSetupProps {
    userType: number,
    navigation: NavigationScreenProp<any, any>
}

export default class NotSetupView extends React.Component<NotSetupProps> {
    constructor(props: NotSetupProps) {
        super(props);
    }


    render() {
        return (
            <View>
                <Image source={NotExistProfile} style={{
                    width: '100%',
                    height: '100%'
                }}/>
                <View style={{
                    position: "absolute",
                    height: '100%',
                    width: '100%'
                }}>
                    <View style={{
                        flexGrow: 1,
                        flexDirection: "row",
                        justifyContent: "center",
                        alignItems: "center"
                    }}>
                        <View style={{
                            padding: 10
                        }}>
                            <Text style={{
                                color: '#FFFFFF',
                                fontSize: 24,
                                textAlign: "center"
                            }}>
                                Setup your profile!
                            </Text>
                            <Text style={{
                                color: '#FFFFFF',
                                fontSize: 16,
                                textAlign: "center"
                            }}>
                                Your profile has not yet been setup. In order for others to find you, the profile has to
                                be
                                setup.
                            </Text>
                            <AtrastiButton title={'Setup now'} onClick={(event) => {
                                if (this.props.userType === UserType_Mod.AGENT) {
                                    this.props.navigation.navigate('ManageAgentView');
                                } else if (this.props.userType === UserType_Mod.COMPANY) {
                                    this.props.navigation.navigate('ManageProfileView');
                                }
                            }} style={Styles.setupButton} textStyle={Styles.setupButtonText}/>
                        </View>
                    </View>
                </View>
            </View>
        );
    }
}

const Styles = StyleSheet.create({
    setupButton: {
        backgroundColor: 'transparent',
        marginTop: 10,
        borderRadius: 50,
        borderColor: '#29B6F6',
        borderWidth: 2
    },
    setupButtonText: {
        textAlign: "center",
        color: "#FFFFFF",
        padding: 10
    },
});
