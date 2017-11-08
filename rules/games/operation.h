#pragma once

/*

In game, all the operations on pieces will be recorded on the list and be
tracked, so that all the history could be reviewed or reversed at anytime.

Every basic change is a change in any attribute of any piece, and is recorded
in one operation log.

Each player step consists of several basic changes, both the piece that causes
the change and all the consequences. They are recorded in one operation page.
Pages will be turned by the controller of the game.

*/

namespace game{

    // Forward declarations
    class TrackableContainer;

    /*
    Defines a trackable attribute for pieces.
    The type T must be comparable and assignable, and must have a default
    */
    template<typename T>
    class Trackable{
    private:
        T _value; // Store the actual value
        const TrackableContainer* _container; // The container containing this trackable
        int _id; // Unique and incremental id as per container

        // Fire a log.
        // Mode is 0: change, 1: construction, 2: destruction
        void _informChange() { _container -> change(); }

    public:
        Trackable(const T& value, TrackableContainer* container): _value(value), _container(container) {}
        Trackable() = delete;

        // Getting
        const T& get() const { return _value; }

        // Setting
        void set(const T& value) { _value = value; _informChange(); }
        void set(T&& value) { _value = value; _informChange(); }

    };

    /*
    Piece should implement this interface
    */
    class TrackableContainer {
    protected:
        int _id; // Unique and incremental Id for the container

        _fireLog

        

    public:
        TrackableContainer(logger)

        ~TrackableContainer() {
            _fireLog
        }
    };

    class TrackableLog {
    public:
        TrackableLog()
    };
    class OperationLog{
    public:
        int containerId;
        int containerAction; // 0: no change, 1: construction, 2: destruction
        int 
    };

    class OperationPage{

    };
}