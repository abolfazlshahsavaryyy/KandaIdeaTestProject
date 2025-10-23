// wwwroot/js/upload.js

const statusDiv = document.getElementById("status");
const cornerMetadata = JSON.parse(sessionStorage.getItem("cornerMetadata"));
const cornerCount = parseInt(sessionStorage.getItem("cornerCount"));
const imageFiles = window.imageFiles || []; // fallback if passed globally

const socket = new WebSocket("ws://localhost:5115/ws");

socket.onopen = async () => {
  statusDiv.innerText = "Connected. Sending corner count...";

  // 1. Send corner count
  socket.send(JSON.stringify({ cornerCount }));

  // 2. Send metadata for each corner
  for (let i = 0; i < cornerCount; i++) {
    const metadata = cornerMetadata[i];
    socket.send(JSON.stringify(metadata));
    statusDiv.innerText += `\nSent metadata for corner ${i + 1}`;
  }

  // 3. Send image for each corner
  for (let i = 0; i < cornerCount; i++) {
    const file = imageFiles[i];
    if (!file) continue;

    // Send image header
    socket.send(JSON.stringify({ cornerIndex: i }));

    // Send image in chunks
    const chunkSize = 4096;
    const arrayBuffer = await file.arrayBuffer();

    for (let offset = 0; offset < arrayBuffer.byteLength; offset += chunkSize) {
      const chunk = arrayBuffer.slice(offset, offset + chunkSize);
      socket.send(chunk);
    }

    statusDiv.innerText += `\nSent image for corner ${i + 1}`;
  }

  socket.close();
};

socket.onclose = () => {
  statusDiv.innerText += "\nUpload complete. WebSocket closed.";
};

socket.onerror = (err) => {
  console.error("WebSocket error:", err);
  statusDiv.innerText = "WebSocket error occurred.";
};
