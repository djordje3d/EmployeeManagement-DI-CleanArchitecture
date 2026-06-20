const { createApp } = Vue; // Import Vue from Vue.js
const vuetify = Vuetify.createVuetify(); // Import Vuetify for UI components

const app = createApp({
    data() { // Data function returns the initial state of the Vue component. Vue objects are reactive, meaning that when the data changes, the UI updates automatically.
        return {
            mesta: [],
            novoMesto: { id: 0, naziv: '', opstina: '', postanskiBroj: '' },
            editMode: false,
            idZaBrisanje: null,
            modalVisible: false,
            confirmDeleteVisible: false,
            sortKey: 'naziv',
            sortAsc: true,
            trenutnaStranica: 1,
            brojPoStranici: 5,
            toastVisible: false, 
            toastPoruka: '',
            toastColor: 'success',
            headers: [
                { text: 'Naziv', value: 'naziv', sortable: true },
                { text: 'Opština', value: 'opstina', sortable: true },
                { text: 'Poštanski broj', value: 'postanskiBroj', sortable: true },
                { text: 'Akcije', value: 'akcije', sortable: false }
            ],
        };
    },
    methods: {
        async ucitajMesta() {
            const response = await fetch('/api/mesta');
            this.mesta = await response.json();
        },
        async dodajMesto() {
            if (!this.novoMesto.naziv) {
                this.prikaziToast("Naziv je obavezan!", 'warning');
                return;
            }

            const poruka = this.editMode ? 'Mesto uspešno izmenjeno!' : 'Novo mesto dodato!';
            const url = this.editMode ? `/api/mesta/${this.novoMesto.id}` : '/api/mesta';
            const method = this.editMode ? 'PUT' : 'POST';

            await fetch(url, {
                method, // Use PUT for updates, POST for new entries
                headers: { 'Content-Type': 'application/json' }, // Set content type to JSON
                body: JSON.stringify(this.novoMesto) // Convert object to JSON string
            });

            this.modalVisible = false;
            this.editMode = false;
            this.novoMesto = { id: 0, naziv: '', opstina: '', postanskiBroj: '' };

            await this.ucitajMesta();
            this.prikaziToast(poruka);
        },
        izmeniMesto(mesto) {
            this.novoMesto = { ...mesto }; // Create a copy of the mesto object
            this.editMode = true; nm3
            this.modalVisible = true; 
        },
        obrisiMesto(id) {
            this.idZaBrisanje = id;
            this.confirmDeleteVisible = true; // Show confirmation modal for deletion
        },
        async potvrdiBrisanje() {
            if (!this.idZaBrisanje) return;

            await fetch(`/api/mesta/${this.idZaBrisanje}`, { method: 'DELETE' }); // Send DELETE request to the API

            this.confirmDeleteVisible = false; // Hide the confirmation modal
            this.idZaBrisanje = null; 

            await this.ucitajMesta();
            this.prikaziToast('Mesto je obrisano.');
        },
        prikaziModal() {
            this.editMode = false;
            this.novoMesto = { id: 0, naziv: '', opstina: '', postanskiBroj: '' };
            this.modalVisible = true;
        },
        sakrijModal() {
            this.modalVisible = false;
        },
        sakrijConfirmDeleteModal() {
            this.confirmDeleteVisible = false;
        },
        prikaziToast(poruka, tip = 'success') {
            this.toastPoruka = poruka;
            this.toastColor = tip === 'error' ? 'red' : tip === 'warning' ? 'orange' : 'green';
            this.toastVisible = true;
        },
        sortirajPo(kolona) {
            if (this.sortKey === kolona) {
                this.sortAsc = !this.sortAsc;
            } else {
                this.sortKey = kolona;
                this.sortAsc = true;
            }
        },
        promeniStranicu(novaStranica) {
            if (novaStranica < 1 || novaStranica > this.ukupnoStranica) return;
            this.trenutnaStranica = novaStranica;
        }
    },
    computed: { // Computed properties for reactive data handling. 
                              //computed properties are cached based on their dependencies, meaning they only re-evaluate when their dependencies change.
        sortiranaMesta() {
            return [...this.mesta].sort((a, b) => {
                let rezultat = 0;
                if (a[this.sortKey] < b[this.sortKey]) rezultat = -1;
                if (a[this.sortKey] > b[this.sortKey]) rezultat = 1;
                return this.sortAsc ? rezultat : -rezultat;
            });
        },
        mestaZaPrikaz() {
            const start = (this.trenutnaStranica - 1) * this.brojPoStranici;
            return this.sortiranaMesta.slice(start, start + this.brojPoStranici);
        },
        ukupnoStranica() {
            return Math.ceil(this.mesta.length / this.brojPoStranici);
        }
    },
    mounted() { // Lifecycle hook that runs when the component is mounted
        this.ucitajMesta();
    }
});

app.use(vuetify); // Use Vuetify for UI components in the Vue app

app.mount('#app'); // Mount the Vue app to the HTML element with id 'app' . 
                   //This is where the Vue instance is initialized and the app starts running.
