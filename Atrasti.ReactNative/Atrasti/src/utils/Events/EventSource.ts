import AtrastiEvent from "./AtrastiEvent";

class EventSourceImpl {

    private readonly _events: Map<String, Array<(event: AtrastiEvent) => void>>;

    constructor() {
        this._events = new Map<String, Array<(event: AtrastiEvent) => void>>();
    }

    subscribe(key: string, event: (event: AtrastiEvent) => void) {
        if(this._events.has(key)) {
            this._events.get(key)?.push(event);
        } else {
            this._events.set(key, [event]);
        }
    }

    unsubscribe(key: string, event: (event: AtrastiEvent) => void) {
        if(!this._events.has(key)) {
            return;
        }

        const eventArray: Array<(event: AtrastiEvent) => void> = this._events.get(key) as Array<(event: AtrastiEvent) => void>;
        const elementIndex = eventArray.indexOf(event);
        if(elementIndex > -1) {
            this._events.get(key)?.splice(elementIndex, 1);
        }
    }

    publish(event: AtrastiEvent) {
        if(!this._events.has(event.type)) {
            return;
        }

        const events = this._events.get(event.type);
        if(events === undefined) return;

        for (const entry of events) {
            entry(event);
        }
    }
}

const EventSource = new EventSourceImpl();
export { EventSource };
