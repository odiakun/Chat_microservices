//state to miejsce, gdzie przechowujesz dane dostepne z kazdego miejsca aplikacji
export const state = () => ({
    messages: []
})
//mutations to funkcje ktore cos dodaja albo zmieniaja w state
export const mutations = {
    newMessage(state,msg) {
        state.messages = [...state.messages, msg];
    },
    clearData(state) {
        state.messages = [];
    },
    DeleteMessage(state, id){
        const index = state.messages.findIndex(object => {
            return object.mid === id;
        });
        state.messages[index].text = "Message deleted";
        state.messages[index].url = null;
    },
    replaceMessage(state, msg){
        const index = state.messages.findIndex(object => {
            return object.mid === msg.mid;
        });
        state.messages.splice(index, 1, msg);
    }
}
//getters to funkcje do wyciagania wiadomosci ze state
export const getters = {
    IsUnique: (state) => (id) => {
        {
            return state.messages.filter(m => m.mid === id).length;
        }
    },
    getMessage: (state) => (id) => {
        {
            return state.messages.filter(m => m.mid === id)[0];
        }
    }
}
