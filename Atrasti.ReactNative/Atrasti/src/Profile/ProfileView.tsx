import { fromNavigationEvent } from "../utils/NavigationEvent";
import { getProfilePage } from "../Api/ProfileAPI";
import CompanyView from "./Views/Company/CompanyView";
import AgentView from "./Views/Agent/AgentView";
import { ActivityIndicator, StyleSheet, View } from "react-native";
import NotSetupView from "./Views/NotSetupView";
import React from "react";
import { NavigationParams, NavigationRoute, NavigationScreenProp } from "react-navigation";
import { CompanyPage_Res } from "../Api/Models/Responds/CompanyPage_Res";
import { User_Res } from "../Api/Models/Responds/User_Res";
import { AgentPage_Res } from "../Api/Models/Responds/AgentPage_Res";

interface ProfileProps {
    navigation: NavigationScreenProp<any, any>;
    route: NavigationRoute;
    ownUser: boolean | null;
}

interface ProfileState {
    company: CompanyPage_Res | null,
    user: User_Res | null,
    isProfileOwner: boolean,
    agent: AgentPage_Res | null,
    setup: boolean,
    userType: number,
    loading: boolean
}

export default class ProfileView extends React.Component<ProfileProps, ProfileState> {

    private _userId: string | null = null;

    constructor(props: ProfileProps) {
        super(props);

        this.state = {
            company: null,
            user: null,
            isProfileOwner: false,
            agent: null,
            setup: true,
            userType: -1,
            loading: true
        }

        if(this.props.ownUser !== undefined && this.props.ownUser) {
            this._userId = null;
        } else {
            if(this.props.route && this.props.route.params && this.props.route.params.userId) {
                this._userId = this.props.route.params.userId;
            }
        }
    }

    componentDidMount() {
        getProfilePage(this._userId).then(result => {
            if(result.setup) {
                if(result.companyPage !== null) {
                    this.setState({
                        user: result.user,
                        company: result.companyPage,
                        isProfileOwner: result.isProfileOwner,
                        userType: result.userType,
                        loading: false
                    });
                } else if(result.agentPage !== null) {
                    this.setState({
                        user: result.user,
                        agent: result.agentPage,
                        isProfileOwner: result.isProfileOwner,
                        userType: result.userType,
                        loading: false
                    });
                }
            } else {
                this.setState({
                    loading: false,
                    setup: false,
                    userType: result.userType
                });
            }

        }).catch(ex => {
            console.log('error:')
            console.log(ex);
        });
    }

    render() {
        let view;
        if(this.state.setup) {
            if(this.state.company !== null) {
                view = <CompanyView navigation={this.props.navigation} products={this.state.company.products}
                                    user={this.state.user} isProfileOwner={this.state.isProfileOwner}/>
            } else if(this.state.agent !== null) {
                view = <AgentView agentPage={this.state.agent} navigation={this.props.navigation}
                                  isProfileOwner={this.state.isProfileOwner} user={this.state.user}/>
            }
        } else if(this.state.loading) {
            view = <ActivityIndicator size={"large"}/>
        } else {
            view = <NotSetupView userType={this.state.userType} navigation={this.props.navigation}/>
        }

        return (
            <View style={Styles.mainView}>
                {view}
            </View>
        );
    }
}

const Styles = StyleSheet.create({
    mainView: {
        flex: 1,
        backgroundColor: '#000000'
    },
});
