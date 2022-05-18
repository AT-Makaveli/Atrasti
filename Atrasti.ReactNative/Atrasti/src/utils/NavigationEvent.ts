import { NavigationEventSubscription } from "react-navigation";

export default class NavigationEvent {
    private readonly _subscription: () => void;

    constructor(subscription: () => void) {
        this._subscription = subscription;
    }

    remove() {
        this._subscription();
    }
}

export function fromNavigationEvent(event: NavigationEventSubscription): NavigationEvent {
    // @ts-ignore
    return new NavigationEvent(event as () => void);
}
