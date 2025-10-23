// wwwroot/js/corners.js

// ✅ Step 1: Extract query parameters
const urlParams = new URLSearchParams(window.location.search);
const landId = urlParams.get("landId");
const cornerCount = parseInt(urlParams.get("cornerCount"));

// ✅ Step 2: Validate parameters
if (!landId || isNaN(cornerCount) || cornerCount < 1) {
  document.getElementById("cornerInputs").innerHTML = "<p style='color:red;'>Invalid land ID or corner count.</p>";
  throw new Error("Missing or invalid query parameters.");
}

// ✅ Step 3: Generate input fields
const cornerInputsDiv = document.getElementById("cornerInputs");

for (let i = 0; i < cornerCount; i++) {
  const cornerDiv = document.createElement("div");
  cornerDiv.innerHTML = `
    <h3>Corner ${i + 1}</h3>
    <label>Latitude:</label>
    <input type="number" step="any" name="latitude${i}" required />
    <label>Longitude:</label>
    <input type="number" step="any" name="longitude${i}" required />
    <label>Image:</label>
    <input type="file" name="image${i}" accept="image/*" required />
    <hr/>
  `;
  cornerInputsDiv.appendChild(cornerDiv);
}

// ✅ Step 4: Handle form submission
document.getElementById("cornerForm").addEventListener("submit", function (e) {
  e.preventDefault();

  const cornerData = [];
  const imageFiles = [];

  for (let i = 0; i < cornerCount; i++) {
    const latitude = parseFloat(e.target[`latitude${i}`].value);
    const longitude = parseFloat(e.target[`longitude${i}`].value);
    const imageFile = e.target[`image${i}`].files[0];

    cornerData.push({
      landId: parseInt(landId),
      latitude,
      longitude,
      cornerIndex: i
    });

    imageFiles.push(imageFile);
  }

  // Store in sessionStorage
  sessionStorage.setItem("cornerMetadata", JSON.stringify(cornerData));
  sessionStorage.setItem("cornerCount", cornerCount);

  // Store image files in a global object (not serializable)
  window.imageFiles = imageFiles;

  // Redirect to upload page
  window.location.href = "upload.html";
});
