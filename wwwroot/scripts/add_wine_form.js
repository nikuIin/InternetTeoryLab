// =================== ЛР 5: Форма добавления вина ===================
// Клиентская валидация (снижает трафик — поля проверяются до отправки)

const validators = {
    Name: function(v) {
        if (!v) return 'Название обязательно';
        if (v.length < 3) return 'Минимум 3 символа';
        if (!/^[\p{L}\s\-']+$/u.test(v)) return 'Только буквы, пробелы, дефисы и апострофы';
        return '';
    },
    WineType: function(v) { return v ? '' : 'Выберите тип вина'; },
    GrapeType: function(v) { return (v && v.length >= 2) ? '' : 'Укажите тип винограда (мин. 2 символа)'; },
    Year: function(v) {
        const n = parseInt(v);
        if (!v) return 'Год обязателен';
        if (isNaN(n) || n < 1800 || n > 2025) return 'Год от 1800 до 2025';
        return '';
    },
    Alcohol: function(v) {
        const n = parseFloat(v);
        if (!v) return 'Алкоголь обязателен';
        if (isNaN(n) || n < 5 || n > 25) return 'Алкоголь от 5.0 до 25.0';
        return '';
    },
    Price: function(v) {
        const n = parseFloat(v);
        if (!v) return 'Цена обязательна';
        if (isNaN(n) || n < 0) return 'Цена не может быть отрицательной';
        return '';
    },
    ProducerEmail: function(v) {
        if (!v) return '';
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(v) ? '' : 'Некорректный email';
    },
    ProducerPhone: function(v) {
        if (!v) return '';
        const phoneRegex = /^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$/;
        return phoneRegex.test(v) ? '' : 'Формат: +7 (XXX) XXX-XX-XX';
    },
    Image: function() { return ''; }
};

function setFieldState(name, error) {
    const el = document.getElementById(name);
    const errEl = document.getElementById('err-' + name);
    if (!el || !errEl) return;
    errEl.textContent = error;
    el.classList.toggle('invalid', !!error);
    el.classList.toggle('valid', !error && el.value);
}

// Навешиваем обработчики после загрузки DOM
document.addEventListener('DOMContentLoaded', function() {

    // Live-валидация при вводе
    Object.keys(validators).forEach(function(name) {
        const el = document.getElementById(name);
        if (!el) return;
        el.addEventListener('blur', function() {
            setFieldState(name, validators[name](el.value.trim()));
        });
        el.addEventListener('input', function() {
            if (el.classList.contains('invalid')) {
                setFieldState(name, validators[name](el.value.trim()));
            }
        });
    });

    // Маска телефона
    const phoneInput = document.getElementById('ProducerPhone');
    if (phoneInput) {
        phoneInput.addEventListener('input', function(e) {
            let v = e.target.value.replace(/\D/g, '');
            if (v.startsWith('7')) v = v.slice(1);
            if (v.startsWith('8')) v = v.slice(1);
            let formatted = '+7 ';
            if (v.length > 0) formatted += '(' + v.slice(0, 3);
            if (v.length >= 3) formatted += ') ' + v.slice(3, 6);
            if (v.length >= 6) formatted += '-' + v.slice(6, 8);
            if (v.length >= 8) formatted += '-' + v.slice(8, 10);
            e.target.value = formatted;
        });
    }

    // Отправка формы через AJAX
    const form = document.getElementById('wine-form');
    if (!form) return;

    form.addEventListener('submit', async function(e) {
        e.preventDefault();

        // Клиентская валидация всех полей перед отправкой
        let hasErrors = false;
        Object.keys(validators).forEach(function(name) {
            const el = document.getElementById(name);
            if (!el) return;
            const err = validators[name](el.value.trim());
            setFieldState(name, err);
            if (err) hasErrors = true;
        });

        if (hasErrors) return;

        const btn = document.getElementById('submit-btn');
        btn.disabled = true;
        btn.textContent = 'Отправка...';

        const formData = new FormData(form);

        try {
            const res = await fetch('/AddWine/Create', {
                method: 'POST',
                body: formData
            });
            const data = await res.json();

            if (data.success) {
                const banner = document.getElementById('success-banner');
                const text = document.getElementById('success-text');
                banner.style.display = 'block';
                text.textContent = ' ' + data.message;
                form.reset();
                document.querySelectorAll('.valid, .invalid').forEach(function(el) {
                    el.classList.remove('valid', 'invalid');
                });
                window.scrollTo({ top: 0, behavior: 'smooth' });
            } else {
                // Серверные ошибки (например, дублирование названия)
                if (data.errors) {
                    Object.entries(data.errors).forEach(function([key, msgs]) {
                        setFieldState(key, msgs[0]);
                    });
                }
            }
        } catch (err) {
            alert('Ошибка связи с сервером. Попробуйте ещё раз.');
        } finally {
            btn.disabled = false;
            btn.textContent = 'Добавить вино в каталог';
        }
    });
});
