let API_BASE = localStorage.getItem('ftApiBase') || 'http://localhost:5029/api';
document.getElementById('apiBase').value = API_BASE;

function saveApiBase() {
  API_BASE = document.getElementById('apiBase').value.trim().replace(/\/$/, '');
  localStorage.setItem('ftApiBase', API_BASE);
  showMsg('URL de l\'API mise à jour.', 'success');
  refreshAll();
}

function showMsg(text, type) {
  const el = document.getElementById('globalMsg');
  el.textContent = text;
  el.className = 'msg ' + type;
  setTimeout(() => { el.className = 'msg'; }, 4000);
}

// --- Navigation ---
document.querySelectorAll('.tab-btn').forEach(btn => {
  btn.addEventListener('click', () => {
    document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
    document.querySelectorAll('.tab-panel').forEach(p => p.classList.remove('active'));
    btn.classList.add('active');
    document.getElementById(btn.dataset.tab).classList.add('active');
  });
});

// --- Helpers ---
async function apiFetch(path, options = {}) {
  const res = await fetch(API_BASE + path, {
    headers: { 'Content-Type': 'application/json' },
    ...options
  });
  if (!res.ok) {
    let detail = '';
    try { detail = await res.text(); } catch (e) {}
    throw new Error(`Erreur ${res.status} : ${detail || res.statusText}`);
  }
  if (res.status === 204) return null;
  return res.json();
}

function escapeHtml(str) {
  return String(str ?? '').replace(/[&<>"']/g, c => ({
    '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;'
  }[c]));
}

// --- Finished Products ---
async function loadFinishedProducts() {
  const container = document.getElementById('fp-list');
  try {
    const items = await apiFetch('/FinishedProducts');
    if (!items || items.length === 0) {
      container.innerHTML = '<div class="empty">Aucun produit fini enregistré.</div>';
      return;
    }
    container.innerHTML = `<table>
      <tr><th>ID</th><th>Nom</th><th>Lot interne</th><th>Date prod.</th><th>Quantité</th><th></th></tr>
      ${items.map(p => `<tr>
        <td>${p.id}</td>
        <td>${escapeHtml(p.name)}</td>
        <td>${escapeHtml(p.internalBatchNumber)}</td>
        <td>${p.productionDate ? new Date(p.productionDate).toLocaleDateString('fr-FR') : ''}</td>
        <td>${p.quantity}</td>
        <td class="actions"><button onclick="deleteItem('/FinishedProducts', ${p.id}, loadFinishedProducts)">Supprimer</button></td>
      </tr>`).join('')}
    </table>`;
  } catch (e) {
    container.innerHTML = `<div class="empty">Impossible de charger les produits finis (${escapeHtml(e.message)})</div>`;
  }
}

async function createFinishedProduct(evt) {
  evt.preventDefault();
  try {
    await apiFetch('/FinishedProducts', {
      method: 'POST',
      body: JSON.stringify({
        name: document.getElementById('fp-name').value,
        internalBatchNumber: document.getElementById('fp-batch').value,
        productionDate: document.getElementById('fp-date').value,
        quantity: Number(document.getElementById('fp-qty').value)
      })
    });
    showMsg('Produit fini ajouté.', 'success');
    evt.target.reset();
    loadFinishedProducts();
  } catch (e) {
    showMsg(e.message, 'error');
  }
  return false;
}

// --- Raw Materials ---
async function loadRawMaterials() {
  const container = document.getElementById('rm-list');
  try {
    const items = await apiFetch('/RawMaterials');
    if (!items || items.length === 0) {
      container.innerHTML = '<div class="empty">Aucune matière première enregistrée.</div>';
      return;
    }
    container.innerHTML = `<table>
      <tr><th>ID</th><th>Nom</th><th>Lot</th><th>Réception</th><th>Péremption</th><th>Statut</th><th>Fournisseur</th><th></th></tr>
      ${items.map(r => `<tr>
        <td>${r.id}</td>
        <td>${escapeHtml(r.name)}</td>
        <td>${escapeHtml(r.batchNumber)}</td>
        <td>${r.receptionDate ? new Date(r.receptionDate).toLocaleDateString('fr-FR') : ''}</td>
        <td>${r.expiryDate ? new Date(r.expiryDate).toLocaleDateString('fr-FR') : ''}</td>
        <td>${escapeHtml(r.status)}</td>
        <td>${r.supplierId}</td>
        <td class="actions"><button onclick="deleteItem('/RawMaterials', ${r.id}, loadRawMaterials)">Supprimer</button></td>
      </tr>`).join('')}
    </table>`;
  } catch (e) {
    container.innerHTML = `<div class="empty">Impossible de charger les matières premières (${escapeHtml(e.message)})</div>`;
  }
}

async function createRawMaterial(evt) {
  evt.preventDefault();
  try {
    await apiFetch('/RawMaterials', {
      method: 'POST',
      body: JSON.stringify({
        name: document.getElementById('rm-name').value,
        batchNumber: document.getElementById('rm-batch').value,
        receptionDate: document.getElementById('rm-reception').value,
        expiryDate: document.getElementById('rm-expiry').value,
        status: document.getElementById('rm-status').value,
        supplierId: Number(document.getElementById('rm-supplier').value)
      })
    });
    showMsg('Matière première ajoutée.', 'success');
    evt.target.reset();
    loadRawMaterials();
  } catch (e) {
    showMsg(e.message, 'error');
  }
  return false;
}

// --- Suppliers ---
async function loadSuppliers() {
  const container = document.getElementById('sp-list');
  try {
    const items = await apiFetch('/Suppliers');
    if (!items || items.length === 0) {
      container.innerHTML = '<div class="empty">Aucun fournisseur enregistré.</div>';
      return;
    }
    container.innerHTML = `<table>
      <tr><th>ID</th><th>Nom</th><th>Email</th><th>Téléphone</th><th>Adresse</th><th></th></tr>
      ${items.map(s => `<tr>
        <td>${s.id}</td>
        <td>${escapeHtml(s.name)}</td>
        <td>${escapeHtml(s.contactEmail)}</td>
        <td>${escapeHtml(s.phone)}</td>
        <td>${escapeHtml(s.address)}</td>
        <td class="actions"><button onclick="deleteItem('/Suppliers', ${s.id}, loadSuppliers)">Supprimer</button></td>
      </tr>`).join('')}
    </table>`;
  } catch (e) {
    container.innerHTML = `<div class="empty">Impossible de charger les fournisseurs (${escapeHtml(e.message)})</div>`;
  }
}

async function createSupplier(evt) {
  evt.preventDefault();
  try {
    await apiFetch('/Suppliers', {
      method: 'POST',
      body: JSON.stringify({
        name: document.getElementById('sp-name').value,
        contactEmail: document.getElementById('sp-email').value,
        phone: Number(document.getElementById('sp-phone').value),
        address: document.getElementById('sp-address').value
      })
    });
    showMsg('Fournisseur ajouté.', 'success');
    evt.target.reset();
    loadSuppliers();
  } catch (e) {
    showMsg(e.message, 'error');
  }
  return false;
}

// --- Traceability Links ---
async function loadLinks() {
  const container = document.getElementById('tl-list');
  try {
    const items = await apiFetch('/TraceabilityLinks');
    if (!items || items.length === 0) {
      container.innerHTML = '<div class="empty">Aucun lien de traçabilité.</div>';
      return;
    }
    container.innerHTML = `<table>
      <tr><th>ID</th><th>Produit fini</th><th>Matière première</th><th>Quantité utilisée</th></tr>
      ${items.map(l => `<tr>
        <td>${l.id}</td>
        <td>${l.finishedProductId}</td>
        <td>${l.rawMaterialId}</td>
        <td>${l.quantityUsed}</td>
      </tr>`).join('')}
    </table>`;
  } catch (e) {
    container.innerHTML = `<div class="empty">Impossible de charger les liens (${escapeHtml(e.message)})</div>`;
  }
}

async function createLink(evt) {
  evt.preventDefault();
  try {
    await apiFetch('/TraceabilityLinks', {
      method: 'POST',
      body: JSON.stringify({
        finishedProductId: Number(document.getElementById('tl-fp').value),
        rawMaterialId: Number(document.getElementById('tl-rm').value),
        quantityUsed: Number(document.getElementById('tl-qty').value)
      })
    });
    showMsg('Lien de traçabilité créé.', 'success');
    evt.target.reset();
    loadLinks();
  } catch (e) {
    showMsg(e.message, 'error');
  }
  return false;
}

async function searchByFinishedProduct(evt) {
  evt.preventDefault();
  const id = document.getElementById('search-fp').value;
  const container = document.getElementById('tl-search-result');
  try {
    const items = await apiFetch(`/TraceabilityLinks/product/${id}`);
    if (!items || items.length === 0) {
      container.innerHTML = '<div class="empty">Aucun résultat.</div>';
      return;
    }
    container.innerHTML = `<table>
      <tr><th>Matière première</th><th>Lot</th><th>Quantité utilisée</th></tr>
      ${items.map(r => `<tr>
        <td>${escapeHtml(r.rawMaterialName)}</td>
        <td>${escapeHtml(r.batchNumber)}</td>
        <td>${r.quantityUsed}</td>
      </tr>`).join('')}
    </table>`;
  } catch (e) {
    container.innerHTML = `<div class="empty">${escapeHtml(e.message)}</div>`;
  }
  return false;
}

// --- Delete générique ---
async function deleteItem(path, id, reload) {
  if (!confirm('Confirmer la suppression ?')) return;
  try {
    await apiFetch(`${path}/${id}`, { method: 'DELETE' });
    showMsg('Élément supprimé.', 'success');
    reload();
  } catch (e) {
    showMsg(e.message, 'error');
  }
}

// --- Init ---
function refreshAll() {
  loadFinishedProducts();
  loadRawMaterials();
  loadSuppliers();
  loadLinks();
}
refreshAll();