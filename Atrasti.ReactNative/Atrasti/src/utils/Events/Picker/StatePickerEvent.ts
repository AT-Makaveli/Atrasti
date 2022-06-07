export default class StatePickerEvent {
    public static EVENT_TYPE: "STATE_PICKER_SELECTED";

    public readonly country: string;
    public readonly state: string;

    constructor(country: string, state: string) {
        this.country = country;
        this.state = state;
    }
}
