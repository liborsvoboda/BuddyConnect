class DVBDeviceBLE {
    listOfFiles = [];
    shortname = null;
    device = null;
    service = null;
    deviceInformation = null;
    serialNumber = null;
    _connectCallback = null;
    _disconnectCallback = null;
    DIS_SERVICE_ID = 'device_information';
    SERIAL_NUMBER_UUID = 'dbd00003-ff30-40a5-9ceb-a17358d31999';
    DVB_SERVICE_UUID = 'dbd00001-ff30-40a5-9ceb-a17358d31999';
    LIST_FILES_UUID = 'dbd00010-ff30-40a5-9ceb-a17358d31999';
    SHORTNAME_UUID = 'dbd00002-ff30-40a5-9ceb-a17358d31999';
    WRITE_TO_DEVICE_UUID = 'dbd00011-ff30-40a5-9ceb-a17358d31999';
    READ_FROM_DEVICE_UUID = 'dbd00012-ff30-40a5-9ceb-a17358d31999';
    FORMAT_STORAGE_UUID = 'dbd00013-ff30-40a5-9ceb-a17358d31999';
    constructor() {
        this.device = null;
        this.service = null;
        this.deviceInformation = null;
        this.serialNumber = null;
        this._connectCallback = null;
        this._disconnectCallback = null;
    }
    // Starts searching for devices and connects to device information and dvb services
    async connect() {
        try {
            const params = {
                optionalServices: [this.DVB_SERVICE_UUID, this.DIS_SERVICE_ID],
                acceptAllDevices: true,
            };
            this.device = await navigator.bluetooth.requestDevice(params);
            this.device.addEventListener('gattserverdisconnected', async (event) => {
                console.log(event);
                await this.disconnected();
            });
            const connection = await this.device.gatt.connect();
            this.service = await connection.getPrimaryService(this.DVB_SERVICE_UUID);
            console.log(`Connected to service ${this.DVB_SERVICE_UUID}`);
            await this.setShortName();
            await this.setSerialNumber();
            await this.setFileList();
            await this.connected();
        }
        catch (error) {
            console.log(error);
            await this.disconnected();
        }
    }
    async connected() {
        if (this._connectCallback)
            this._connectCallback();
    }
    onConnect(callback) {
        this._connectCallback = callback;
        return this;
    }
    // disconnectes device and sets everything to null or empty array
    disconnect() {
        if (this.device) {
            return this.device.gatt.disconnect();
        }
    }
    async disconnected() {
        console.log('Disconnected');
        if (this._disconnectCallback)
            this._disconnectCallback();
        this.device = null;
        this.service = null;
        this.serialNumber = null;
        this.listOfFiles = [];
    }
    onDisconnect(callback) {
        this._disconnectCallback = callback;
        return this;
    }
    // retrieves shortname from DVB unit
    async getShortName() {
        return this.shortname;
    }
    // with parameter you set a new shortname. without parameter you retrieve the current shortname from device and set this.shortname
    async setShortName(shortname) {
        try {
            if (!shortname) {
                const characteristic = await this.service.getCharacteristic(this.SHORTNAME_UUID);
                const value = await characteristic.readValue();
                const shortName = new TextDecoder().decode(value);
                this.shortname = shortName;
            }
            else {
                const characteristic = await this.service.getCharacteristic(this.SHORTNAME_UUID);
                const uf8encode = new TextEncoder();
                const newShortName = uf8encode.encode(shortname);
                await characteristic.writeValue(newShortName);
                this.shortname = newShortName;
            }
        }
        catch (error) { }
    }
    // retrieve array buffers
    getFileList() {
        return this.listOfFiles;
    }
    // gets files from device and sets this.listOfFIles
    async setFileList() {
        try {
            while (true) {
                const characteristic = await this.service.getCharacteristic(this.LIST_FILES_UUID);
                const value = await characteristic.readValue();
                const message = new Uint8Array(value.buffer);
                if (message.byteLength === 0)
                    return;
                const byteString = String.fromCharCode(...message);
                const split_string = byteString.split(';');
                const name = split_string[0];
                const length = split_string[1];
                this.listOfFiles.push({ name, length });
            }
        }
        catch (error) {
            console.log(error);
        }
    }
    // retrieves the data from file using parameter name
    async getFileContent(name) {
        try {
            const write_characteristic = await this.service.getCharacteristic(this.WRITE_TO_DEVICE_UUID);
            const read_characteristic = await this.service.getCharacteristic(this.READ_FROM_DEVICE_UUID);
            const arrayBuffers = [];
            let offset = 0;
            const uf8encode = new TextEncoder();
            const name_bytes = uf8encode.encode(`${name};${offset};`);
            await write_characteristic.writeValue(name_bytes);
            while (true) {
                const display_info = await read_characteristic.readValue();
                if (display_info.byteLength !== 0) {
                    offset += display_info.byteLength;
                    console.log(`Appending length to offset: ${offset}`);
                    const uf8encode = new TextEncoder();
                    const name_bytes = uf8encode.encode(`${name};${offset};`);
                    await write_characteristic.writeValue(name_bytes);
                    const array = new Uint8Array(display_info.buffer);
                    array.map((x) => {
                        arrayBuffers.push(x);
                    });
                }
                else {
                    break;
                }
            }
            return new Uint8Array(arrayBuffers);
        }
        catch (error) {
            console.log(error);
        }
    }
    // retrieves serial number
    getSerialNumber() {
        console.log(`Serial Number: ${this.serialNumber}`);
        return this.serialNumber;
    }
    // retrieves current serial number and sets this.serialNumber
    async setSerialNumber() {
        try {
            const characteristic = await this.service.getCharacteristic(this.SERIAL_NUMBER_UUID);
            const serial = await characteristic.readValue();
            const serialNumber = new TextDecoder().decode(serial);
            this.serialNumber = serialNumber;
        }
        catch (error) {
            console.log(error);
        }
    }
    // formats storage
    async formatStorage() {
        try {
            const characteristic = await this.service.getCharacteristic(this.FORMAT_STORAGE_UUID);
            await characteristic.readValue();
            console.log('Files erased');
        }
        catch (error) {
            console.log(error);
        }
    }
}

