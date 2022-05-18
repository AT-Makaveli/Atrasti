export default class StatePickerEvent {
    public static EVENT_TYPE: "STATE_PICKER_SELECTED";

    public readonly state: string;

    constructor(country: string) {
        this.state = country;
    }
}
