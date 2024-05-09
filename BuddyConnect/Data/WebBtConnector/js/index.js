const connectButton = document.getElementById('connect');
const disconnectButton = document.getElementById('disconnect');
const logButton = document.getElementById('log');
const saveButton = document.getElementById('save');
const bluetoothIsAvailable = document.getElementById('bluetooth-is-available');
const table = document.getElementById('table');
const logInfo = document.getElementById('log-info');
const fileInfo = document.getElementById('file-info');
const fileTitle = document.querySelector('.file-title');
const spinner = document.getElementById('spinner');
const shortname = document.getElementById('set-shortname');
const shortnameTitle = document.getElementById('title-shortname');
const formatButton = document.getElementById('format-button');
let hex_string = '';
let content = null;
const connection = new DVBDeviceBLE();
const toggleButtons = (connected) => {
    if (connected) {
        connectButton.style.display = 'none';
        disconnectButton.style.display = 'block';
    }
    else {
        connectButton.style.display = 'block';
        disconnectButton.style.display = 'none';
    }
};
const removeRows = function () {
    for (var i = 1; i < table.rows.length;) {
        table.deleteRow(i);
    }
};
const scanDevices = async () => {
    try {
        spinner.style.display = 'block';
        await connection.connect();
        const files = connection.getFileList();
        if (files.length > 0) {
            console.log('files', files);
            files.map((file) => {
                generateTableRows(file.name, file.length);
            });
        }
        spinner.style.display = 'none';
        toggleButtons(true);
    }
    catch (error) {
        printLog(`Error: ${error}`);
        spinner.style.display = 'none';
        toggleButtons(false);
    }
};
const saveFile = () => {
    const name = fileTitle.textContent;
    const file = new Blob([content]);
    const link = document.createElement('a');
    link.href = URL.createObjectURL(file);
    link.download = name;
    link.click();
    URL.revokeObjectURL(link.href);
};
const printLog = (message) => {
    const now = new Date();
    const date = `${now.getFullYear()}-${now.getMonth() + 1}-${now.getDate()}`;
    const time = `${now.getHours()}:${now.getMinutes()}:${now.getSeconds()}`;
    if (message.includes('Error')) {
        logInfo.innerHTML += `<p class="red">${date} ${time} - ${message}</p>`;
        logButton.style.color = 'red';
    }
    else {
        logInfo.innerHTML += `<p>${date} ${time} - ${message}</p>`;
    }
};
async function downloadFile(e) {
    fileInfo.textContent = '';
    try {
        const name = e.target.name;
        if (name === undefined) {
            fileTitle.textContent = `Error.Please close and try again`;
        }
        else {
            fileTitle.textContent = `Loading Content for ${name}`;
        }
        content = await connection.getFileContent(name);
        hex_string = content.map((x) => x.toString()).join('');
        fileInfo.style.display = 'block';
        fileInfo.textContent = hex_string;
        fileTitle.textContent = name;
    }
    catch (error) {
        printLog(`Error: ${error}`);
    }
}
const generateTableRows = (name, length) => {
    const row = table.insertRow(-1);
    const cellName = row.insertCell(0);
    const cellLength = row.insertCell(1);
    const cellButtons = row.insertCell(2);
    cellName.textContent = name;
    cellLength.textContent = length;
    const icon = document.createElement('i');
    icon.className = 'fa fa-download';
    const span = document.createElement('span');
    span.style.fontSize = 'smaller';
    span.textContent = 'Download';
    const buttonDownload = document.createElement('button');
    buttonDownload.className = 'btn btn-sm btn-dark table-button';
    buttonDownload.setAttribute('data-bs-toggle', 'modal');
    buttonDownload.setAttribute('data-bs-target', '#file-content');
    buttonDownload.name = name;
    buttonDownload.textContent = 'Download';
    cellButtons.appendChild(buttonDownload);
    buttonDownload.addEventListener('click', downloadFile);
};
if (navigator && navigator.bluetooth && navigator.bluetooth.getAvailability()) {
    toggleButtons(false);
}
else {
    bluetoothIsAvailable.style.display = 'block';
}
connectButton.addEventListener('click', scanDevices);
saveButton.addEventListener('click', saveFile);
disconnectButton.addEventListener('click', () => {
    connection.disconnect();
    console.log('Disconnected');
    toggleButtons(false);
});
logButton.addEventListener('click', () => {
    logButton.style.color = 'white';
});
shortname.addEventListener('keydown', async (e) => {
    if (e.key === 'Enter') {
        let value = e.target.value;
        await connection.setShortName(value);
        shortnameTitle.innerText = value;
        shortname.value = '';
        console.log(`New Shortname: ${value}`);
    }
});
formatButton.addEventListener('click', async () => {
    const userConfirm = confirm('Are you sure you want to format device?');
    try {
        if (userConfirm) {
            await connection.formatStorage();
            alert('Device formatted');
            connection.disconnect();
        }
    }
    catch (error) {
        alert('There was some error formatting your device.');
        console.log('Format:', error);
    }
});
connection.onDisconnect(function () {
    removeRows();
});
connection.onConnect(async function () {
    const shortname = await connection.getShortName();
    shortnameTitle.innerText = shortname;
    console.log('Serial Number', connection.serialNumber);
});
