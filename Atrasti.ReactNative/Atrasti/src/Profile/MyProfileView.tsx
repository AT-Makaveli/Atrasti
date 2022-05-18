import React from "react";
import { NavigationRoute, NavigationScreenProp } from "react-navigation";
import NavigationEvent, { fromNavigationEvent } from "../utils/NavigationEvent";
import { User_Res } from "../Api/Models/Responds/User_Res";
import { AgentPage_Res } from "../Api/Models/Responds/AgentPage_Res";
import { CompanyPage_Res } from "../Api/Models/Responds/CompanyPage_Res";
import { StyleSheet} from "react-native";
import ProfileView from "./ProfileView";

interface MyProfileProps {
    navigation: NavigationScreenProp<any, any>;
    route: NavigationRoute;
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

export default class MyProfileView extends React.Component<MyProfileProps, ProfileState> {

    private _onMyProfileViewBeingShown: NavigationEvent | null = null;
    private _profileView: ProfileView | null = null;
    private _userId: string | null = null;

    constructor(props: MyProfileProps) {
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

        if(this.props.route && this.props.route.params && this.props.route.params.userId) {
            this._userId = this.props.route.params.userId.toString();
        }
    }

    componentDidMount() {
        const event = this.props.navigation.addListener('focus', this.onRequestMyProfile.bind(this));
        this._onMyProfileViewBeingShown = fromNavigationEvent(event);
    }

    onRequestMyProfile() {
        this._profileView?.componentDidMount();
    }

    componentWillUnmount() {
        this._onMyProfileViewBeingShown?.remove();
    }

    render() {
        return (
            <ProfileView navigation={this.props.navigation} ownUser={true} route={this.props.route} ref={(ref) => this._profileView = ref}/>
        );
    }
}
