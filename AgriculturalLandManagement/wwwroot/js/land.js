// wwwroot/js/land.js
async function fetchLand() {
  const landId = document.getElementById("landIdInput").value;
  const response = await fetch(`/api/Land/${landId}`);///api/Land/{id}

  if (response.ok) {
    const land = await response.json();
    document.getElementById("landDetails").textContent = JSON.stringify(land, null, 2);
    document.getElementById("landInfo").style.display = "block";
    document.getElementById("errorMessage").textContent = "";
  } else {
    document.getElementById("errorMessage").textContent = "Land not found.";
    document.getElementById("landInfo").style.display = "none";
  }
}

function goToCornerPage() {
  const landId = document.getElementById("landIdInput").value;
  const cornerCount = document.getElementById("cornerCountInput").value;
  window.location.href = `corners.html?landId=${landId}&cornerCount=${cornerCount}`;
}
